using System;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO.Compression;
using System.Net.Security;
using System.Reflection;
using System.Text;
using System.Xml;
using Xenoverse;
using XVModManager.Properties;

namespace XVModManager
{
    public partial class Form1 : Form
    {
        CPK cpk = new CPK();
        string data_path;
        string flex_sdk_path;
        string xenoverse_path;
        string lang;
        public Form1()
        {
            InitializeComponent();
        }
        private void Form1_Load(object sender, EventArgs e)
        {

            FolderBrowserDialog datadialog = new FolderBrowserDialog();
            datadialog.Description = "Select \"DB Xenoverse\" folder";

            FolderBrowserDialog flexsdkdialog = new FolderBrowserDialog();
            datadialog.Description = "Select \"Flex SDK 4.6\" folder";

            if (Directory.Exists("C:\\Program Files (x86)\\Steam\\steamapps\\common\\DB Xenoverse"))
            {
                Settings.Default.Xenoverse_path = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\DB Xenoverse";
                Settings.Default.data_path = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\DB Xenoverse\\data";
                Settings.Default.Save();

            }
            if (Directory.Exists("C:\\flexsdk"))
            {
                Settings.Default.flex_sdk_path = "C:\\flexsdk";
                Settings.Default.Save();
            }

            if (Settings.Default.flex_sdk_path.Length == 0 || Settings.Default.data_path.Length == 0)
            {
                if (datadialog.ShowDialog() == DialogResult.OK)
                {
                    Settings.Default.data_path = datadialog.SelectedPath + @"/data";
                    Settings.Default.Xenoverse_path = datadialog.SelectedPath;
                    Settings.Default.Save();
                }
                if (flexsdkdialog.ShowDialog() == DialogResult.OK)
                {
                    Settings.Default.flex_sdk_path = flexsdkdialog.SelectedPath;
                    Settings.Default.Save();
                }
            }

            data_path = Settings.Default.data_path;
            xenoverse_path = Settings.Default.Xenoverse_path;
            flex_sdk_path = Settings.Default.flex_sdk_path;


            Xenoverse.Xenoverse.AURFile = Settings.Default.data_path + @"/aura_setting.aur";
            Xenoverse.Xenoverse.CMSFile = Settings.Default.data_path + @"/system/char_model_spec.cms";
            Xenoverse.Xenoverse.CSOFile = Settings.Default.data_path + @"/system/chara_sound.cso";
            Xenoverse.Xenoverse.CUSFile = Settings.Default.data_path + @"/system/custom_skill.cus";
            Xenoverse.Xenoverse.PSCFile = Settings.Default.data_path + @"/system/parameter_spec_char.psc";
            Xenoverse.Xenoverse.CHARASELE_IGGY = Settings.Default.data_path + @"/ui/iggy/CHARASELE.iggy";
            Xenoverse.Xenoverse.STAGESELE_IGGY = Settings.Default.data_path + @"/ui/iggy/STAGESELE.iggy";
            Xenoverse.Xenoverse.proper_noun_character_name = Settings.Default.data_path + @"/msg/proper_noun_character_name_" + lang + ".msg";
            Xenoverse.Xenoverse.proper_noun_costume_name = Settings.Default.data_path + @"/msg/proper_noun_costume_name_en_" + lang + ".msg";

            if (!File.Exists(Settings.Default.Xenoverse_path + "/" + Xenoverse.Xenoverse.xvpatcher_dll))
            {
                MessageBox.Show("XVPatcher not detected, XVModManager can't work without it.",
                "Warning", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this.Close();
            }
            loadLvItems();

            foreach (string item in Xenoverse.Xenoverse.languages)
                toolStripComboBox1.Items.Add(item);

            if (Settings.Default.language.Length == 0)
            {
                lang = "en";
                toolStripComboBox1.SelectedItem = "en";
                Settings.Default.language = "en";
                Settings.Default.Save();
            }

            toolStripComboBox1.SelectedItem = Settings.Default.language;
            lang = toolStripComboBox1.SelectedItem.ToString();


            if (!Directory.Exists(Settings.Default.data_path))
            {
                Directory.CreateDirectory(Settings.Default.data_path);

                BinaryReader br = new BinaryReader(File.OpenRead(xenoverse_path + "/" + Xenoverse.Xenoverse.data2_cpk));
                cpk.ReadCPK(xenoverse_path + "/" + Xenoverse.Xenoverse.data2_cpk);

                extractfilefromCPK("data/ui/iggy/CHARASELE.iggy", br);

                extractfilefromCPK("data/system/aura_setting.aur", br);
                extractfilefromCPK("data/system/chara_sound.cso", br);
                extractfilefromCPK("data/system/char_model_spec.cms", br);
                extractfilefromCPK("data/system/custom_skill.cus", br);
                extractfilefromCPK("data/system/parameter_spec_char.psc", br);

                extractfilefromCPK("data/msg/proper_noun_character_name_" + lang + ".msg", br);
                extractfilefromCPK("data/msg/proper_noun_costume_name_" + lang + ".msg", br);
                extractfilefromCPK("data/msg/proper_noun_skill_spa_name_" + lang + ".msg", br);
                extractfilefromCPK("data/msg/proper_noun_skill_ult_name_" + lang + ".msg", br);
                extractfilefromCPK("data/msg/proper_noun_skill_esc_name_" + lang + ".msg", br);

                extractfilefromCPK("data/ui/texture/CHARA01.emb", br);

                var myAssembly = Assembly.GetExecutingAssembly();
                var myStream = myAssembly.GetManifestResourceStream("XVModManager.ZipFile_Blobs.scripts.zip");
                ZipArchive archive = new ZipArchive(myStream);
                archive.ExtractToDirectory(data_path);

                var myAssembly2 = Assembly.GetExecutingAssembly();
                var myStream2 = myAssembly2.GetManifestResourceStream("XVModManager.ZipFile_Blobs.iggy_as3_test.zip");
                ZipArchive archive2 = new ZipArchive(myStream2);
                archive2.ExtractToDirectory(Path.Combine(data_path + @"\ui\iggy"));

                var myAssembly3 = Assembly.GetExecutingAssembly();
                var myStream3 = myAssembly3.GetManifestResourceStream("XVModManager.ZipFile_Blobs.embpack.zip");
                ZipArchive archive3 = new ZipArchive(myStream3);
                archive3.ExtractToDirectory(Path.Combine(data_path + @"\ui\texture"));

                var myAssembly4 = Assembly.GetExecutingAssembly();
                var myStream4 = myAssembly4.GetManifestResourceStream("XVModManager.ZipFile_Blobs.XMLSerializer.zip");
                ZipArchive archive4 = new ZipArchive(myStream4);
                archive4.ExtractToDirectory(Path.Combine(data_path + @"\system"));

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
                        sw.WriteLine("cd " + data_path + @"\ui\texture");
                        sw.WriteLine(@"embpack.exe CHARA01.emb");
                    }
                }

            }
            Clean();
        }
        private void saveLvItems()
        {
            Settings.Default.modlist = new StringCollection();
            Settings.Default.modlist.AddRange((from i in this.lvMods.Items.Cast<ListViewItem>()
                                               select string.Join("|", from si in i.SubItems.Cast<ListViewItem.ListViewSubItem>()
                                                                       select si.Text)).ToArray());
            Settings.Default.Save();
        }

        private void loadLvItems()
        {
            if (Settings.Default.modlist == null)
            {
                Settings.Default.modlist = new StringCollection();
            }

            this.lvMods.Items.AddRange((from i in Settings.Default.modlist.Cast<string>()
                                        select new ListViewItem(i.Split('|'))).ToArray());
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
            short VOX_1 = -1;
            short VOX_2 = -1;

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
                                if (reader.Name == "VOX_1")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        bool parseSuccess = Int16.TryParse(reader.GetAttribute("value").Trim(), out VOX_1);
                                        if (!parseSuccess)
                                        {
                                            // Gestisci il caso in cui la conversione non riesce, ad esempio, fornisci un valore predefinito o mostra un messaggio di errore.
                                            MessageBox.Show("VOX_1 value not recognized", "Error", MessageBoxButtons.OK);
                                            return;
                                        }
                                    }
                                }
                                if (reader.Name == "VOX_2")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        bool parseSuccess = Int16.TryParse(reader.GetAttribute("value").Trim(), out VOX_2);
                                        if (!parseSuccess)
                                        {
                                            // Gestisci il caso in cui la conversione non riesce, ad esempio, fornisci un valore predefinito o mostra un messaggio di errore.
                                            MessageBox.Show("VOX_2 value not recognized", "Error", MessageBoxButtons.OK);
                                            return;
                                        }
                                    }
                                }

                            }
                        }

                    }
                    if (modtype == "REPLACER")
                    {

                        MergeDirectoriesWithConfirmation(Xenoverse.Xenoverse.temp_path, data_path);

                        Clean();

                        MessageBox.Show("Mod installed successfully", "Success", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        string[] row = { modname, modauthor, "Replacer" };
                        ListViewItem lvi = new ListViewItem(row);
                        lvMods.Items.Add(lvi);
                        saveLvItems();
                    }
                    else if (modtype == "ADDED_CHARACTER")
                    {
                        int CharID = 108 + Settings.Default.modlist.Count;
                        MergeDirectoriesWithConfirmation(Xenoverse.Xenoverse.temp_path, data_path);

                        Clean();

                        // CMS
                        CMS cms = new CMS();
                        cms.Load(Xenoverse.Xenoverse.CMSFile);
                        CharacterData newCharacter = new CharacterData
                        {
                            ID = CharID, // ID del personaggio
                            ShortName = CMS_BCS, // Nome abbreviato del personaggio
                            Unknown = new byte[8], // Array di byte sconosciuto
                            Paths = new string[7] // Array di percorsi
                        };
                        newCharacter.Paths[0] = CMS_BCS;
                        newCharacter.Paths[1] = CMS_EAN;
                        newCharacter.Paths[2] = CMS_FCE_EAN;
                        newCharacter.Paths[3] = CMS_CAM_EAN;
                        newCharacter.Paths[4] = CMS_BAC;
                        newCharacter.Paths[5] = CMS_BCM;
                        newCharacter.Paths[6] = CMS_BAI;
                        cms.AddCharacter(newCharacter);

                        // CSO
                        CSO cso = new CSO();
                        cso.Load(Xenoverse.Xenoverse.CSOFile);
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
                        cso.AddCharacter(characterData);

                        // CUS
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
                                sw.WriteLine("cd " + data_path + @"\system");
                                sw.WriteLine(@"CUSXMLSerializer.exe custom_skill.cus");
                            }
                        }
                        p.WaitForExit();

                        string cuspath = data_path + @"\system\custom_skill.cus.xml";
                        string text4 = File.ReadAllText(cuspath);

                        text4 = text4.Replace("  </Skillsets>", "    <Skillset Character_ID=\"" + CharID + $"\" Costume_Index=\"0\" Model_Preset=\"0\">\r\n      <SuperSkill1 ID1=\"{CUS_SUPER_1}\" />\r\n      <SuperSkill2 ID1=\"{CUS_SUPER_2}\" />\r\n      <SuperSkill3 ID1=\"{CUS_SUPER_3}\" />\r\n      <SuperSkill4 ID1=\"{CUS_SUPER_4}\" />\r\n      <UltimateSkill1 ID1=\"{CUS_ULTIMATE_1}\" />\r\n      <UltimateSkill2 ID1=\"{CUS_ULTIMATE_2}\" />\r\n      <EvasiveSkill ID1=\"{CUS_EVASIVE}\" />\r\n      <BlastType ID1=\"65535\" />\r\n      <AwokenSkill ID1=\"0\" />\r\n    </Skillset>\r\n  </Skillsets>");
                        File.WriteAllText(cuspath, text4);

                        p.Start();

                        using (StreamWriter sw = p.StandardInput)
                        {
                            if (sw.BaseStream.CanWrite)
                            {
                                const string quote = "\"";

                                sw.WriteLine("cd " + data_path + @"\system");
                                sw.WriteLine(@"CUSXMLSerializer.exe " + quote + data_path + @"\system\custom_skill.cus.xml" + quote);
                            }
                        }

                        p.WaitForExit();

                        // AUR
                        p.Start();
                        using (StreamWriter sw = p.StandardInput)
                        {
                            if (sw.BaseStream.CanWrite)
                            {
                                sw.WriteLine("cd " + data_path + @"\system");
                                sw.WriteLine(@"AURXMLSerializer.exe aura_setting.aur");
                            }
                        }
                        p.WaitForExit();

                        string aurpath = data_path + @"\system\aura_setting.aur.xml";
                        string text5 = File.ReadAllText(aurpath);
                        string glare;
                        if (AUR_GLARE == 1)
                        {
                            glare = "True";
                        }
                        else
                        {
                            glare = "False";
                        }
                        text5 = text5.Replace("  </CharacterAuras>", "    <CharacterAura Chara_ID=\"" + CharID + $"\" Costume=\"0\" Aura_ID=\"{glare}\" Glare=\"False\" />\r\n  </CharacterAuras>");
                        File.WriteAllText(aurpath, text5);

                        p.Start();

                        using (StreamWriter sw = p.StandardInput)
                        {
                            if (sw.BaseStream.CanWrite)
                            {
                                const string quote = "\"";

                                sw.WriteLine("cd " + data_path + @"\system");
                                sw.WriteLine(@"AURXMLSerializer.exe " + quote + data_path + @"\system\aura_setting.aur.xml" + quote);
                            }
                        }

                        p.WaitForExit();


                        //////

                        // PSC
                        p.Start();
                        using (StreamWriter sw = p.StandardInput)
                        {
                            if (sw.BaseStream.CanWrite)
                            {
                                sw.WriteLine("cd " + data_path + @"\system");
                                sw.WriteLine(@"PSCXMLSerializer.exe parameter_spec_char.psc");
                            }
                        }
                        p.WaitForExit();

                        string pscpath = data_path + @"\system\parameter_spec_char.psc.xml";
                        string text6 = File.ReadAllText(pscpath);

                        text6 = text6.Replace("  </Configuration>\r\n</PSC>", "    <PSC_Entry Chara_ID=\"" + CharID + "\">\r\n      <PscSpecEntry Costume=\"0\" Preset=\"0\">\r\n        <Camera_Position value=\"1\" />\r\n        <I_12 value=\"5\" />\r\n        <Health value=\"1.1155\" />\r\n        <F_20 value=\"1.0\" />\r\n        <Ki value=\"1.0\" />\r\n        <Ki_Recharge value=\"1.0\" />\r\n        <I_32 value=\"1\" />\r\n        <I_36 value=\"1\" />\r\n        <I_40 value=\"0\" />\r\n        <Stamina value=\"1.5\" />\r\n        <Stamina_Recharge value=\"0.75\" />\r\n        <F_52 value=\"1.0\" />\r\n        <F_56 value=\"1.1\" />\r\n        <I_60 value=\"0\" />\r\n        <Basic_Atk_Defense value=\"1.0\" />\r\n        <Basic_Ki_Defense value=\"0.95\" />\r\n        <Strike_Atk_Defense value=\"1.1\" />\r\n        <Super_Ki_Defense value=\"0.95\" />\r\n        <Ground_Speed value=\"1.0\" />\r\n        <Air_Speed value=\"1.0\" />\r\n        <Boost_Speed value=\"1.0\" />\r\n        <Dash_Speed value=\"1.0\" />\r\n        <F_96 value=\"1.0\" />\r\n        <Reinforcement_Skill_Duration value=\"1.0\" />\r\n        <F_104 value=\"1.0\" />\r\n        <Revival_HP_Amount value=\"1.0\" />\r\n        <Reviving_Speed value=\"1.0\" />\r\n        <F_116 value=\"1.0\" />\r\n        <F_120 value=\"0.55\" />\r\n        <F_124 value=\"1.0\" />\r\n        <F_128 value=\"1.0\" />\r\n        <F_132 value=\"1.0\" />\r\n        <F_136 value=\"1.0\" />\r\n        <I_140 value=\"0\" />\r\n        <F_144 value=\"1.0\" />\r\n        <F_148 value=\"1.0\" />\r\n        <F_152 value=\"1.0\" />\r\n        <F_156 value=\"1.0\" />\r\n        <F_160 value=\"1.0\" />\r\n        <F_164 value=\"1.0\" />\r\n        <Z-Soul value=\"98\" />\r\n        <I_172 value=\"1\" />\r\n        <I_176 value=\"1\" />\r\n        <F_180 value=\"8.0\" />\r\n      </PscSpecEntry>\r\n    </PSC_Entry>\r\n  </Configuration>\r\n</PSC>");
                        File.WriteAllText(pscpath, text6);

                        p.Start();

                        using (StreamWriter sw = p.StandardInput)
                        {
                            if (sw.BaseStream.CanWrite)
                            {
                                const string quote = "\"";

                                sw.WriteLine("cd " + data_path + @"\system");
                                sw.WriteLine(@"PSCXMLSerializer.exe " + quote + data_path + @"\system\parameter_spec_char.psc.xml" + quote);
                            }
                        }

                        p.WaitForExit();
                        //////

                        string Charalist = data_path + @"\scripts\action_script\Charalist.as";

                        var text10 = new StringBuilder();

                        foreach (string s in File.ReadAllLines(Charalist))
                        {
                            text10.AppendLine(s.Replace("[[\"JCO\",0,0,0,[110,111]]]", "[[\"JCO\",0,0,0,[110,111]]],[[\"" + CMS_BCS + $"\",0,0,0,[{VOX_1},{VOX_2}]]]"));
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

                        p.Start();

                        using (StreamWriter sw = p.StandardInput)
                        {
                            if (sw.BaseStream.CanWrite)
                            {
                                sw.WriteLine("cd " + data_path + @"\ui\texture");
                                sw.WriteLine(@"embpack.exe CHARA01");
                            }
                        }

                        string[] row = { modname, modauthor, "Added Character" };
                        ListViewItem lvi = new ListViewItem(row);
                        lvMods.Items.Add(lvi);
                        saveLvItems();

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
        }


        private void CompileScripts()
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            Process process = new Process();
            string sourcepath = "\"" + data_path + "\\scripts\"";
            string maintimelinepath = "\"" + data_path + "\\scripts\\dlc3_CHARASELE_fla\\MainTimeline.as\"";

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
                    standardInput.WriteLine("cd " + flex_sdk_path + "\\bin");
                    standardInput.WriteLine("mxmlc -compiler.source-path=" + sourcepath + " " + maintimelinepath);
                }
            }
            process.WaitForExit();
            Directory.CreateDirectory(data_path + "\\ui\\iggy\\");

            if (File.Exists(data_path + "\\ui\\iggy\\CHARASELE.swf"))
                File.Delete(data_path + "\\ui\\iggy\\CHARASELE.swf");


            File.Move(data_path + "\\scripts\\dlc3_CHARASELE_fla\\MainTimeline.swf", data_path + "\\ui\\iggy\\CHARASELE.swf");

            Thread.Sleep(1000);
            process.Start();
            using (StreamWriter standardInput = process.StandardInput)
            {
                if (standardInput.BaseStream.CanWrite)
                {
                    standardInput.WriteLine("cd " + data_path + @"\ui\iggy");
                    standardInput.WriteLine("iggy_as3_test.exe CHARASELE.swf");
                }
            }
            process.WaitForExit();

            Thread.Sleep(1000);

            if (File.Exists(data_path + "\\ui\\iggy\\CHARASELE.swf"))
                File.Delete(data_path + "\\ui\\iggy\\CHARASELE.swf");
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
            if (Directory.Exists(data_path + @"/xvmod.xml"))
                Directory.Delete(data_path + @"/xvmod.xml", true);
        }


        private void clearInstallationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to clear your mod installation?", "Warning",
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (Directory.Exists(data_path))
                    Directory.Delete(data_path, true);
                Settings.Default.Reset();
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

        private void toolstripcbbox1_selectedindexchanged(object sender, EventArgs e)
        {
            lang = toolStripComboBox1.SelectedItem.ToString();
            Settings.Default.language = toolStripComboBox1.SelectedItem.ToString();
            Settings.Default.Save();

            BinaryReader br = new BinaryReader(File.OpenRead(xenoverse_path + "/" + Xenoverse.Xenoverse.data2_cpk));
            cpk.ReadCPK(xenoverse_path + "/" + Xenoverse.Xenoverse.data2_cpk);
            extractfilefromCPK("data/msg/proper_noun_character_name_" + lang + ".msg", br);
            extractfilefromCPK("data/msg/proper_noun_costume_name_" + lang + ".msg", br);
            extractfilefromCPK("data/msg/proper_noun_skill_spa_name_" + lang + ".msg", br);
            extractfilefromCPK("data/msg/proper_noun_skill_ult_name_" + lang + ".msg", br);
            extractfilefromCPK("data/msg/proper_noun_skill_esc_name_" + lang + ".msg", br);
        }
    }
}