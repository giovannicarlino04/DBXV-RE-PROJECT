using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using XVReborn.Properties;

namespace XVReborn
{
    public partial class Form1 : Form
    {
        string language = "";

        public Form1()
        {
            InitializeComponent();
        }

        public void Form1_Load(object sender, EventArgs e)
        {
            if (Settings.Default.language.Length == 0)
            {
                Form2 frm = new Form2();
                frm.ShowDialog();
                language = Settings.Default.language;
            }
            else
            {
                language = Settings.Default.language;
            }

            if (Properties.Settings.Default.datafolder.Length == 0 || Properties.Settings.Default.flexsdkfolder.Length == 0)
            {
                Form3 frm = new Form3();
                frm.ShowDialog();
            }
            else
            {
                if (Directory.Exists(Settings.Default.datafolder) == false)
                {
                    MessageBox.Show("Data Folder not Found, Please Clear Installation", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            if (File.Exists(Properties.Settings.Default.datafolder + @"\ui\texture\embpack.exe") == false)
            {
                var myAssembly = Assembly.GetExecutingAssembly();
                var myStream = myAssembly.GetManifestResourceStream("XVReborn.ZipFile_Blobs.embpack.zip");
                ZipArchive archive = new ZipArchive(myStream);
                archive.ExtractToDirectory(Path.Combine(Settings.Default.datafolder + @"\ui\texture"));
            }

            if (Directory.Exists(Settings.Default.datafolder + @"\ui\texture\CHARA01") == false)
            {
                if (Directory.Exists(Settings.Default.datafolder + @"\ui\texture") == false)
                {
                    Directory.CreateDirectory(Settings.Default.datafolder + @"\ui\texture");
                }

                var myAssembly = Assembly.GetExecutingAssembly();
                var myStream = myAssembly.GetManifestResourceStream("XVReborn.ZipFile_Blobs.CHARA01.zip");
                ZipArchive archive = new ZipArchive(myStream);
                archive.ExtractToDirectory(Path.Combine(Settings.Default.datafolder + @"\ui\texture"));

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
                        sw.WriteLine("cd " + Properties.Settings.Default.datafolder + @"\ui\texture");
                        sw.WriteLine(@"embpack.exe CHARA01.emb");
                    }
                }
            }

            if (Directory.Exists(Properties.Settings.Default.datafolder + @"\system") == false)
            {
                Directory.CreateDirectory(Properties.Settings.Default.datafolder + @"\system");

                var myAssembly = Assembly.GetExecutingAssembly();

                var myStream = myAssembly.GetManifestResourceStream("XVReborn.ZipFile_Blobs.char_model_spec.zip");
                var myStream2 = myAssembly.GetManifestResourceStream("XVReborn.ZipFile_Blobs.chara_sound.zip");
                var myStream3 = myAssembly.GetManifestResourceStream("XVReborn.ZipFile_Blobs.parameter_spec_char.zip");
                var myStream4 = myAssembly.GetManifestResourceStream("XVReborn.ZipFile_Blobs.aura_setting.zip");
                var myStream5 = myAssembly.GetManifestResourceStream("XVReborn.ZipFile_Blobs.custom_skill.zip");
                var myStream6 = myAssembly.GetManifestResourceStream("XVReborn.ZipFile_Blobs.XMLSerializer.zip");
                var myStream7 = myAssembly.GetManifestResourceStream("XVReborn.ZipFile_Blobs.item.zip");

                ZipArchive archive = new ZipArchive(myStream);
                ZipArchive archive2 = new ZipArchive(myStream2);
                ZipArchive archive3 = new ZipArchive(myStream3);
                ZipArchive archive4 = new ZipArchive(myStream4);
                ZipArchive archive5 = new ZipArchive(myStream5);
                ZipArchive archive6 = new ZipArchive(myStream6);
                ZipArchive archive7 = new ZipArchive(myStream7);

                archive.ExtractToDirectory(Path.Combine(Settings.Default.datafolder + @"\system"));
                archive2.ExtractToDirectory(Path.Combine(Settings.Default.datafolder + @"\system"));
                archive3.ExtractToDirectory(Path.Combine(Settings.Default.datafolder + @"\system"));
                archive4.ExtractToDirectory(Path.Combine(Settings.Default.datafolder + @"\system"));
                archive5.ExtractToDirectory(Path.Combine(Settings.Default.datafolder + @"\system"));
                archive6.ExtractToDirectory(Path.Combine(Settings.Default.datafolder + @"\system"));
                archive7.ExtractToDirectory(Path.Combine(Settings.Default.datafolder + @"\system"));
            }

            if (Directory.Exists(Properties.Settings.Default.datafolder + @"\ui\iggy") == false)
            {
                Directory.CreateDirectory(Properties.Settings.Default.datafolder + @"\system");

                var myAssembly = Assembly.GetExecutingAssembly();

                var myStream = myAssembly.GetManifestResourceStream("XVReborn.ZipFile_Blobs.CHARASELE.zip");
                var myStream2 = myAssembly.GetManifestResourceStream("XVReborn.ZipFile_Blobs.iggy_as3_test.zip");

                ZipArchive archive = new ZipArchive(myStream);
                ZipArchive archive2 = new ZipArchive(myStream2);

                archive.ExtractToDirectory(Path.Combine(Settings.Default.datafolder + @"\ui\iggy"));
                archive2.ExtractToDirectory(Path.Combine(Settings.Default.datafolder + @"\ui\iggy"));
            }

            if (Directory.Exists(Properties.Settings.Default.datafolder + @"\msg") == false)
            {
                var myAssembly = Assembly.GetExecutingAssembly();
                var myStream = myAssembly.GetManifestResourceStream("XVReborn.ZipFile_Blobs.msg.zip");
                ZipArchive archive = new ZipArchive(myStream);
                archive.ExtractToDirectory(Path.Combine(Settings.Default.datafolder + @"\msg"));
            }

            if (Directory.Exists(Properties.Settings.Default.datafolder + @"\scripts") == false)
            {
                var myAssembly = Assembly.GetExecutingAssembly();
                var myStream = myAssembly.GetManifestResourceStream("XVReborn.ZipFile_Blobs.scripts.zip");
                ZipArchive archive = new ZipArchive(myStream);
                archive.ExtractToDirectory(Settings.Default.datafolder);
            }

            if (Properties.Settings.Default.modlist.Contains("System.Object"))
            {
                Properties.Settings.Default.modlist.Clear();
            }

            if (Properties.Settings.Default.addonmodlist.Contains("System.Object"))
            {
                Properties.Settings.Default.addonmodlist.Clear();
            }
            loadLvItems();
        }

        private void CompileScripts()
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            Process process = new Process();
            StringBuilder stringBuilder = new StringBuilder();
            string sourcepath = "\"" + Properties.Settings.Default.datafolder + "\\scripts\"";
            string maintimelinepath = "\"" + Properties.Settings.Default.datafolder + "\\scripts\\dlc3_CHARASELE_fla\\MainTimeline.as\"";

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
                    standardInput.WriteLine("cd " + Properties.Settings.Default.flexsdkfolder + "\\bin");
                    standardInput.WriteLine("mxmlc -compiler.source-path=" + sourcepath + " -omit-trace-statements=false " + maintimelinepath);
                }
            }
            process.WaitForExit();
            Directory.CreateDirectory(Properties.Settings.Default.datafolder + "\\ui\\iggy\\");

            if (File.Exists(Properties.Settings.Default.datafolder + "\\ui\\iggy\\CHARASELE.swf"))
                File.Delete(Properties.Settings.Default.datafolder + "\\ui\\iggy\\CHARASELE.swf");


            File.Move(Properties.Settings.Default.datafolder + "\\scripts\\dlc3_CHARASELE_fla\\MainTimeline.swf", Properties.Settings.Default.datafolder + "\\ui\\iggy\\CHARASELE.swf");

            Thread.Sleep(1000);
            process.Start();
            using (StreamWriter standardInput = process.StandardInput)
            {
                if (standardInput.BaseStream.CanWrite)
                {
                    standardInput.WriteLine("cd " + Settings.Default.datafolder + @"\ui\iggy");
                    standardInput.WriteLine("iggy_as3_test.exe CHARASELE.swf");
                }
            }
            process.WaitForExit();

            Thread.Sleep(1000);

            if (File.Exists(Properties.Settings.Default.datafolder + "\\ui\\iggy\\CHARASELE.swf"))
                File.Delete(Properties.Settings.Default.datafolder + "\\ui\\iggy\\CHARASELE.swf");
        }

        private void saveLvItems()
        {
            Properties.Settings.Default.modlist = new StringCollection();
            Properties.Settings.Default.modlist.AddRange((from i in this.listView1.Items.Cast<ListViewItem>()
                                                          select string.Join("|", from si in i.SubItems.Cast<ListViewItem.ListViewSubItem>()
                                                                                  select si.Text)).ToArray());
            Properties.Settings.Default.Save();
            label1.Text = "Installed Mods: " + listView1.Items.Count.ToString();
        }

        private void loadLvItems()
        {
            if (Properties.Settings.Default.modlist == null)
            {
                Properties.Settings.Default.modlist = new StringCollection();
            }

            this.listView1.Items.AddRange((from i in Properties.Settings.Default.modlist.Cast<string>()
                                        select new ListViewItem(i.Split('|'))).ToArray());

            label1.Text = "Installed Mods: " + listView1.Items.Count.ToString();
        }
        private void installmod(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Install Mod";
            ofd.Filter = "Xenoverse Mod Files (*.xvmod)|*.xvmod";
            ofd.Multiselect = true;
            ofd.CheckFileExists = true;

            string modtype = "";
            string modname = "";
            string modauthor = "";
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
                    string xmlfile = "./XVRebornTemp" + @"/xvmod.xml";

                    if (File.Exists(Settings.Default.datafolder + @"/xvmod.xml"))
                        File.Delete(Settings.Default.datafolder + @"/xvmod.xml");
                    if (File.Exists(xmlfile))
                        File.Delete(xmlfile);
                    ZipFile.ExtractToDirectory(file, "./XVRebornTemp");

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
                        if (File.Exists(xmlfile))
                            File.Delete(xmlfile);
                    }
                    if (modtype == "REPLACER")
                    {

                        MergeDirectoriesWithConfirmation("./XVRebornTemp", Settings.Default.datafolder);

                        Clean();

                        MessageBox.Show("Mod installed successfully", "Success", MessageBoxButtons.OK,
                            MessageBoxIcon.Information);

                        string[] row = { modname, modauthor, "Replacer" };
                        ListViewItem lvi = new ListViewItem(row);
                        listView1.Items.Add(lvi);
                        saveLvItems();
                    }
                    else if (modtype == "ADDED_CHARACTER")
                    {
                        int CharID = 108 + Settings.Default.modlist.Count;
                        MergeDirectoriesWithConfirmation("./XVRebornTemp", Settings.Default.datafolder);

                        Clean();

                        // CMS
                        CMS cms = new CMS();
                        cms.Load(Settings.Default.datafolder + @"/system/char_model_spec.cms");
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
                        cso.Load(Settings.Default.datafolder + @"/system/chara_sound.cso");
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
                                sw.WriteLine("cd " + Settings.Default.datafolder + @"\system");
                                sw.WriteLine(@"CUSXMLSerializer.exe custom_skill.cus");
                            }
                        }
                        p.WaitForExit();

                        string cuspath = Settings.Default.datafolder + @"\system\custom_skill.cus.xml";
                        string text4 = File.ReadAllText(cuspath);

                        text4 = text4.Replace("  </Skillsets>", "    <Skillset Character_ID=\"" + CharID + $"\" Costume_Index=\"0\" Model_Preset=\"0\">\r\n      <SuperSkill1 ID1=\"{CUS_SUPER_1}\" />\r\n      <SuperSkill2 ID1=\"{CUS_SUPER_2}\" />\r\n      <SuperSkill3 ID1=\"{CUS_SUPER_3}\" />\r\n      <SuperSkill4 ID1=\"{CUS_SUPER_4}\" />\r\n      <UltimateSkill1 ID1=\"{CUS_ULTIMATE_1}\" />\r\n      <UltimateSkill2 ID1=\"{CUS_ULTIMATE_2}\" />\r\n      <EvasiveSkill ID1=\"{CUS_EVASIVE}\" />\r\n      <BlastType ID1=\"65535\" />\r\n      <AwokenSkill ID1=\"0\" />\r\n    </Skillset>\r\n  </Skillsets>");
                        File.WriteAllText(cuspath, text4);

                        p.Start();

                        using (StreamWriter sw = p.StandardInput)
                        {
                            if (sw.BaseStream.CanWrite)
                            {
                                const string quote = "\"";

                                sw.WriteLine("cd " + Settings.Default.datafolder + @"\system");
                                sw.WriteLine(@"CUSXMLSerializer.exe " + quote + Settings.Default.datafolder + @"\system\custom_skill.cus.xml" + quote);
                            }
                        }

                        p.WaitForExit();

                        // AUR
                        p.Start();
                        using (StreamWriter sw = p.StandardInput)
                        {
                            if (sw.BaseStream.CanWrite)
                            {
                                sw.WriteLine("cd " + Settings.Default.datafolder + @"\system");
                                sw.WriteLine(@"AURXMLSerializer.exe aura_setting.aur");
                            }
                        }
                        p.WaitForExit();

                        string aurpath = Settings.Default.datafolder + @"\system\aura_setting.aur.xml";
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
                        text5 = text5.Replace("  </CharacterAuras>", "    <CharacterAura Chara_ID=\"" + CharID + $"\" Costume=\"0\" Aura_ID=\"{AUR_ID}\" Glare=\"{glare}\" />\r\n  </CharacterAuras>");
                        File.WriteAllText(aurpath, text5);

                        p.Start();

                        using (StreamWriter sw = p.StandardInput)
                        {
                            if (sw.BaseStream.CanWrite)
                            {
                                const string quote = "\"";

                                sw.WriteLine("cd " + Settings.Default.datafolder + @"\system");
                                sw.WriteLine(@"AURXMLSerializer.exe " + quote + Settings.Default.datafolder + @"\system\aura_setting.aur.xml" + quote);
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
                                sw.WriteLine("cd " + Settings.Default.datafolder + @"\system");
                                sw.WriteLine(@"PSCXMLSerializer.exe parameter_spec_char.psc");
                            }
                        }
                        p.WaitForExit();

                        string pscpath = Settings.Default.datafolder + @"\system\parameter_spec_char.psc.xml";
                        string text6 = File.ReadAllText(pscpath);

                        text6 = text6.Replace("  </Configuration>\r\n</PSC>", "    <PSC_Entry Chara_ID=\"" + CharID + "\">\r\n      <PscSpecEntry Costume=\"0\" Preset=\"0\">\r\n        <Camera_Position value=\"1\" />\r\n        <I_12 value=\"5\" />\r\n        <Health value=\"1.1155\" />\r\n        <F_20 value=\"1.0\" />\r\n        <Ki value=\"1.0\" />\r\n        <Ki_Recharge value=\"1.0\" />\r\n        <I_32 value=\"1\" />\r\n        <I_36 value=\"1\" />\r\n        <I_40 value=\"0\" />\r\n        <Stamina value=\"1.5\" />\r\n        <Stamina_Recharge value=\"0.75\" />\r\n        <F_52 value=\"1.0\" />\r\n        <F_56 value=\"1.1\" />\r\n        <I_60 value=\"0\" />\r\n        <Basic_Atk_Defense value=\"1.0\" />\r\n        <Basic_Ki_Defense value=\"0.95\" />\r\n        <Strike_Atk_Defense value=\"1.1\" />\r\n        <Super_Ki_Defense value=\"0.95\" />\r\n        <Ground_Speed value=\"1.0\" />\r\n        <Air_Speed value=\"1.0\" />\r\n        <Boost_Speed value=\"1.0\" />\r\n        <Dash_Speed value=\"1.0\" />\r\n        <F_96 value=\"1.0\" />\r\n        <Reinforcement_Skill_Duration value=\"1.0\" />\r\n        <F_104 value=\"1.0\" />\r\n        <Revival_HP_Amount value=\"1.0\" />\r\n        <Reviving_Speed value=\"1.0\" />\r\n        <F_116 value=\"1.0\" />\r\n        <F_120 value=\"0.55\" />\r\n        <F_124 value=\"1.0\" />\r\n        <F_128 value=\"1.0\" />\r\n        <F_132 value=\"1.0\" />\r\n        <F_136 value=\"1.0\" />\r\n        <I_140 value=\"0\" />\r\n        <F_144 value=\"1.0\" />\r\n        <F_148 value=\"1.0\" />\r\n        <F_152 value=\"1.0\" />\r\n        <F_156 value=\"1.0\" />\r\n        <F_160 value=\"1.0\" />\r\n        <F_164 value=\"1.0\" />\r\n        <Z-Soul value=\"98\" />\r\n        <I_172 value=\"1\" />\r\n        <I_176 value=\"1\" />\r\n        <F_180 value=\"8.0\" />\r\n      </PscSpecEntry>\r\n    </PSC_Entry>\r\n  </Configuration>\r\n</PSC>");
                        File.WriteAllText(pscpath, text6);

                        p.Start();

                        using (StreamWriter sw = p.StandardInput)
                        {
                            if (sw.BaseStream.CanWrite)
                            {
                                const string quote = "\"";

                                sw.WriteLine("cd " + Settings.Default.datafolder + @"\system");
                                sw.WriteLine(@"PSCXMLSerializer.exe " + quote + Settings.Default.datafolder + @"\system\parameter_spec_char.psc.xml" + quote);
                            }
                        }

                        p.WaitForExit();
                        //////

                        // IGGY
                        string Charalist = Properties.Settings.Default.datafolder + @"\scripts\action_script\Charalist.as";

                        var text10 = new StringBuilder();

                        foreach (string s in File.ReadAllLines(Charalist))
                        {
                            text10.AppendLine(s.Replace("[[\"JCO\",0,0,0,[110,111]]]", "[[\"JCO\",0,0,0,[110,111]]],[[\"" + CMS_BCS + "\",0,0,0,[" + VOX_1 + "," + VOX_2 + "]]]"));
                        }

                        using (var IggyFile = new StreamWriter(File.Create(Charalist)))
                        {
                            IggyFile.Write(text10.ToString());
                        }

                        CompileScripts();

                        /////

                        // MSG
                        msg MSGfile;
                        MSGfile = msgStream.Load(Settings.Default.datafolder + @"/msg/proper_noun_character_name_" + language + ".msg");
                        msgData[] expand = new msgData[MSGfile.data.Length + 1];
                        Array.Copy(MSGfile.data, expand, MSGfile.data.Length);
                        string nameid = MSGfile.data[MSGfile.data.Length - 1].NameID;
                        int endid = int.Parse(nameid.Substring(nameid.Length - 3, 3));
                        expand[expand.Length - 1].ID = MSGfile.data.Length;
                        expand[expand.Length - 1].Lines = new string[] { MSG_CHARACTER_NAME };
                        expand[expand.Length - 1].NameID = "chara_" + CMS_BCS + "_" + (endid).ToString("000");

                        MSGfile.data = expand;

                        msgStream.Save(MSGfile, Settings.Default.datafolder + @"/msg/proper_noun_character_name_" + language + ".msg");
                        ////

                        // EMB
                        p.Start();

                        using (StreamWriter sw = p.StandardInput)
                        {
                            if (sw.BaseStream.CanWrite)
                            {
                                sw.WriteLine("cd " + Settings.Default.datafolder + @"\ui\texture");
                                sw.WriteLine(@"embpack.exe CHARA01");
                            }
                        }
                        ////

                        string[] row = { modname, modauthor, "Added Character" };
                        ListViewItem lvi = new ListViewItem(row);
                        listView1.Items.Add(lvi);
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
            Clean();
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
            if (File.Exists(Properties.Settings.Default.datafolder + "//modinfo.xml"))
            {
                File.Delete(Properties.Settings.Default.datafolder + "//modinfo.xml");
            }

            if (Directory.Exists(Properties.Settings.Default.datafolder + "//temp"))
            {
                Directory.Delete(Properties.Settings.Default.datafolder + "//temp", true);
            }

            if (File.Exists(Properties.Settings.Default.datafolder + @"\system\aura_setting.aur.xml"))
            {
                File.Delete(Properties.Settings.Default.datafolder + @"\system\aura_setting.aur.xml");
            }

            if (File.Exists(Properties.Settings.Default.datafolder + @"\system\aura_setting.aur.xml.bak"))
            {
                File.Delete(Properties.Settings.Default.datafolder + @"\system\aura_setting.aur.xml.bak");
            }

            if (File.Exists(Properties.Settings.Default.datafolder + @"\system\custom_skill.cus.xml"))
            {
                File.Delete(Properties.Settings.Default.datafolder + @"\system\custom_skill.cus.xml");
            }

            if (File.Exists(Properties.Settings.Default.datafolder + @"\system\custom_skill.cus.xml.bak"))
            {
                File.Delete(Properties.Settings.Default.datafolder + @"\system\custom_skill.cus.xml.bak");
            }

            if (File.Exists(Properties.Settings.Default.datafolder + @"\system\char_model_spec.cms.xml"))
            {
                File.Delete(Properties.Settings.Default.datafolder + @"\system\char_model_spec.cms.xml");
            }

            if (File.Exists(Properties.Settings.Default.datafolder + @"\system\char_model_spec.cms.xml.bak"))
            {
                File.Delete(Properties.Settings.Default.datafolder + @"\system\char_model_spec.cms.xml.bak");
            }

            if (File.Exists(Properties.Settings.Default.datafolder + @"\system\parameter_spec_char.psc.xml"))
            {
                File.Delete(Properties.Settings.Default.datafolder + @"\system\parameter_spec_char.psc.xml");
            }

            if (File.Exists(Properties.Settings.Default.datafolder + @"\system\parameter_spec_char.psc.xml.bak"))
            {
                File.Delete(Properties.Settings.Default.datafolder + @"\system\parameter_spec_char.psc.xml.bak");
            }

            if (File.Exists(Properties.Settings.Default.datafolder + @"\system\chara_sound.cso.xml"))
            {
                File.Delete(Properties.Settings.Default.datafolder + @"\system\chara_sound.cso.xml");
            }

            if (File.Exists(Properties.Settings.Default.datafolder + @"\system\chara_sound.cso.xml.bak"))
            {
                File.Delete(Properties.Settings.Default.datafolder + @"\system\chara_sound.cso.xml.bak");
            }

            if (Directory.Exists("./XVRebornTemp"))
                Directory.Delete("./XVRebornTemp", true);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Clean();
        }
        private void clearInstallationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to clear installation?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (Directory.Exists(Settings.Default.datafolder))
                {
                    Directory.Delete(Settings.Default.datafolder, true);
                }

                if (Directory.Exists(Settings.Default.datafolder + @"\scripts"))
                {
                    Directory.Delete(Settings.Default.datafolder + @"\scripts", true);
                }

                Settings.Default.Reset();
                MessageBox.Show("Installation cleared, XVReborn will now close", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                return;
            }
        }

        private void compileScriptsAdvancedToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CompileScripts();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
