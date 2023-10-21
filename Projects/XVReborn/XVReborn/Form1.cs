using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using XVReborn.Properties;

namespace XVReborn
{
    struct Aura
    {
        public int[] Color;
    }

    struct Charlisting
    {
        public int Name;
        public int Costume;
        public int ID;
        public bool inf;
    }

    struct CharName
    {
        public int ID;
        public string Name;

        public CharName(int i, string n)
        {
            ID = i;
            Name = n;
        }
    }

    public partial class Form1 : Form
    {
        public class DraggableButton : Button
        {
            public DraggableButton()
            {
                this.AllowDrop = true;
            }
        }

        string[][][] charaList; // Class-level variable to store the parsed character data.

        string AURFileName;
        Aura[] Auras;
        Charlisting[] Chars;
        byte[] backup = new byte[104];
        bool AuraLock = false;
        bool CharLock = false;
        CharName[] clist = new CharName[] {
            new CharName(0, "Goku"),
            new CharName(1, "Bardock"),
            new CharName(2, "Goku SSJ4"),
            new CharName(3, "Goku SSJGod"),
            new CharName(4, "Goku GT"),
            new CharName(5, "Goten"),
            new CharName(6, "Gohan kid"),
            new CharName(7, "Gohan Teen"),
            new CharName(8, "Gohan Adult"),
            new CharName(9, "Piccolo"),
            new CharName(10, "Krillin"),
            new CharName(11, "Yamcha"),
            new CharName(12, "Tien"),
            new CharName(13, "Raditz"),
            new CharName(14, "Saibaman"),
            new CharName(15, "Nappa"),
            new CharName(16, "Vegeta"),
            new CharName(17, "Vegeta SSJ4"),
            new CharName(18, "Guldo"),
            new CharName(19, "Burter"),
            new CharName(20, "Recoome"),
            new CharName(21, "Jeice"),
            new CharName(22, "Ginyu"),
            new CharName(23, "Frieza 1st Form"),
            new CharName(24, "Frieza Final"),
            new CharName(25, "Frieza Full Power"),
            new CharName(26, "Trunks Future"),
            new CharName(27, "Trunks Kid"),
            new CharName(28, "Android 17"),
            new CharName(29, "Super 17"),
            new CharName(30, "Android 18"),
            new CharName(31, "Cell Perfect"),
            new CharName(32, "Cell Full Power"),
            new CharName(33, "Cell Jr."),
            new CharName(34, "Videl"),
            new CharName(35, "Majin Buu"),
            new CharName(36, "Super Buu"),
            new CharName(37, "Kid Buu"),
            new CharName(38, "Gotenks"),
            new CharName(39, "Vegito"),
            new CharName(40, "Broly"),
            new CharName(41, "Beerus"),
            new CharName(42, "Pan"),
            new CharName(48, "Eis Shenron"),
            new CharName(49, "Nuova Shenron"),
            new CharName(50, "Omega Shenron"),
            new CharName(51, "Gogeta SSJ4"),
            new CharName(52, "Hercule"),
            new CharName(53, "Demigra"),
            new CharName(59, "Nabana"),
            new CharName(60, "Raspberry"),
            new CharName(61, "Gohan 4 years old"),
            new CharName(62, "Mira"),
            new CharName(63, "Towa"),
            new CharName(65, "Whis"),
            new CharName(67, "Jaco"),
            new CharName(73, "Villinous Hercule"),
            new CharName(80, "Goku SSGSS"),
            new CharName(81, "Vegeta SSGSS"),
            new CharName(82, "Golden Frieza"),
            new CharName(100, "Human Male"),
            new CharName(101, "Human Female"),
            new CharName(102, "Saiyan Male"),
            new CharName(103, "Saiyan Female"),
            new CharName(104, "Namekian"),
            new CharName(105, "Frieza Race"),
            new CharName(106, "Majin Male"),
            new CharName(107, "Majin Female")
            };

        msg Chartxt;
        CMS cmsfile = new CMS();
        PSC pFile = new PSC();
        CharSkill CS = new CharSkill();
        CSO csoFile = new CSO();
        int[] CostumeIndex = { 0, 0 };
        public msg file;
        string MSGFileName;
        string IDBFileName;
        List<string> sf = new List<string>();

        string[] ListNames = {"Health","Ki","Ki Recovery","Stamina",
            "Stamina Recovery","Enemy Stamina Eraser", "Unknown 1","Ground Speed",
        "Air Speed", "Dash Speed", "Unknown 2", "Normal Attack Damage",
            "Normal Ki Blast Damage","Super Attack Damage","Super Ki Blasts", "Physical Damage Received",
        "Ki Damage Received", "Physical Recharge Damage Received", "Ki Recharge Damage Received","Transform Duration",
        "Reinforcement Skills Duration","Unknown 3","Revival HP Amount","Unknown 4",
        "Ally Revival Speed"};
        EffectList eList;
        ActivationList aList;
        idbItem[] Items;
        string FileNameMsgN;
        msg Names;
        string FileNameMsgD;
        msg Descs;
        bool lockMod = false;
        int copy;

        string language = "";

        public Form1()
        {
            InitializeComponent();
            foreach (string str in ListNames)
            {
                var Item = new ListViewItem(new[] { str, "1.0" });
                var Item1 = new ListViewItem(new[] { str, "1.0" });
                var Item2 = new ListViewItem(new[] { str, "1.0" });
                lstvBasic.Items.Add(Item);
                lstvEffect1.Items.Add(Item1);
                lstvEffect2.Items.Add(Item2);
            }
        }

        public void Form1_Load(object sender, EventArgs e)
        {
            if (Directory.Exists("C:\\Program Files (x86)\\Steam\\steamapps\\common\\DB Xenoverse"))
            {
                if (!Directory.Exists("C:\\Program Files (x86)\\Steam\\steamapps\\common\\DB Xenoverse\\data"))
                    Directory.CreateDirectory("C:\\Program Files (x86)\\Steam\\steamapps\\common\\DB Xenoverse\\data");
                Properties.Settings.Default.datafolder = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\DB Xenoverse\\data";
                Properties.Settings.Default.Save();
            }
            if (Directory.Exists("C:\\flexsdk"))
            {
                Properties.Settings.Default.flexsdkfolder = "C:\\flexsdk";
                Properties.Settings.Default.Save();
            }
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
                if (Directory.Exists(Properties.Settings.Default.datafolder) == false)
                {
                    MessageBox.Show("Data Folder not Found, Please Clear Installation", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                if (!File.Exists(Path.Combine(Settings.Default.datafolder + @"/../xvpatcher.exe")))
                {
                    if (MessageBox.Show("XVPatcher or one of it's components is missing, please install it, otherwise mods won't work!!!", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                    }
                }
            }

            if (File.Exists(Properties.Settings.Default.datafolder + @"\ui\texture\embpack.exe") == false)
            {
                var myAssembly = Assembly.GetExecutingAssembly();
                var myStream = myAssembly.GetManifestResourceStream("XVReborn.ZipFile_Blobs.embpack.zip");
                ZipArchive archive = new ZipArchive(myStream);
                archive.ExtractToDirectory(Path.Combine(Settings.Default.datafolder + @"\ui\texture"));
            }

            if (Directory.Exists(Properties.Settings.Default.datafolder + @"\ui\texture\CHARA01") == false)
            {
                if (Directory.Exists(Properties.Settings.Default.datafolder + @"\ui\texture") == false)
                {
                    Directory.CreateDirectory(Properties.Settings.Default.datafolder + @"\ui\texture");
                }

                var myAssembly = Assembly.GetExecutingAssembly();
                var myStream = myAssembly.GetManifestResourceStream("XVReborn.ZipFile_Blobs.CHARA01.zip");
                ZipArchive archive = new ZipArchive(myStream);

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

            loadFiles();

        }

        private void loadFiles()
        {
            lvMods.Items.Clear();
            loadLvItems();

            MSGFileName = Properties.Settings.Default.datafolder + @"\msg\proper_noun_character_name_" + language + ".msg";
            file = msgStream.Load(MSGFileName);

            cbList.Items.Clear();
            for (int i = 0; i < file.data.Length; i++)
                cbList.Items.Add(file.data[i].ID.ToString() + " - " + file.data[i].NameID);

            cmsfile.Load(Properties.Settings.Default.datafolder + @"/system" + "/char_model_spec.cms");

            Chartxt = msgStream.Load(Properties.Settings.Default.datafolder + "/msg/proper_noun_character_name_" + language + ".msg");

            cbCharacter.Items.Clear();
            foreach (CMS_Data cd in cmsfile.Data)
            {
                string name = Chartxt.Find("chara_" + cd.ShortName + "_000");
                if (name == "No Matching ID")
                    cbCharacter.Items.Add("Unknown Character");
                else
                    cbCharacter.Items.Add(name);
            }

            pFile.load(Properties.Settings.Default.datafolder + @"/system" + "/parameter_spec_char.psc");

            PSClstData.Clear();
            foreach (string str in pFile.ValNames)
            {
                var Item = new ListViewItem(new[] { str, "0" });
                PSClstData.Items.Add(Item);
            }
            CS.populateSkillData(Properties.Settings.Default.datafolder + @"/msg", Properties.Settings.Default.datafolder + @"/system" + "/custom_skill.cus", language);

            //populate skill lists
            foreach (skill sk in CS.Supers)
            {
                SupLst1.Items.Add(sk.Name);
                SupLst2.Items.Add(sk.Name);
                SupLst3.Items.Add(sk.Name);
                SupLst4.Items.Add(sk.Name);
            }

            foreach (skill sk in CS.Ultimates)
            {
                UltLst1.Items.Add(sk.Name);
                UltLst2.Items.Add(sk.Name);
            }

            foreach (skill sk in CS.Evasives)
            {
                EvaLst.Items.Add(sk.Name);
            }

            csoFile.Load(Properties.Settings.Default.datafolder + @"/system" + "/chara_sound.cso");

            AURFileName = Properties.Settings.Default.datafolder + @"/system/aura_setting.aur";
            byte[] AURfile = File.ReadAllBytes(AURFileName);

            //Aura Editor
            Auras = new Aura[BitConverter.ToInt32(AURfile, 8)];
            int AuraAddress = BitConverter.ToInt32(AURfile, 12);
            for (int A = 0; A < Auras.Length; A++)
            {
                int id = BitConverter.ToInt32(AURfile, AuraAddress + (16 * A));
                Auras[id].Color = new int[BitConverter.ToInt32(AURfile, AuraAddress + (16 * A) + 8)];
                int CAddress = BitConverter.ToInt32(AURfile, AuraAddress + (16 * A) + 12);
                for (int C = 0; C < Auras[id].Color.Length; C++)
                    Auras[id].Color[BitConverter.ToInt32(AURfile, CAddress + (C * 8))] = BitConverter.ToInt32(AURfile, CAddress + (C * 8) + 4);
            }

            for (int A = 0; A < Auras.Length; A++)
                cbAuraList.Items.Add(A);

            //backup this data up
            int WAddress = BitConverter.ToInt32(AURfile, 20);
            Array.Copy(AURfile, WAddress, backup, 0, 104);

            //Character Aura Changer
            cbAURChar.Items.Clear();
            Chars = new Charlisting[BitConverter.ToInt32(AURfile, 24)];
            int ChAddress = BitConverter.ToInt32(AURfile, 28);
            for (int C = 0; C < Chars.Length; C++)
            {
                Chars[C].Name = BitConverter.ToInt32(AURfile, ChAddress + (C * 16));
                Chars[C].Costume = BitConverter.ToInt32(AURfile, ChAddress + (C * 16) + 4);
                Chars[C].ID = BitConverter.ToInt32(AURfile, ChAddress + (C * 16) + 8);
                Chars[C].inf = BitConverter.ToBoolean(AURfile, ChAddress + (C * 16) + 12);

                cbAURChar.Items.Add(FindCharName(Chars[C].Name) + " - Costume " + Chars[C].Costume.ToString());
            }

            //Install mod opening the .x1m file
            string[] args = Environment.GetCommandLineArgs();

            foreach (string arg in args)
            {
                if (arg.EndsWith(".x1m"))
                {
                    if (MessageBox.Show("Do you want to install \"" + Path.GetFileNameWithoutExtension(arg) + "\" ?", "Mod Installation", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        installmod(arg);
                    }
                }

            }

            //Load the default idb file
            loadidbfile("talisman", Settings.Default.datafolder + @"/system/item/talisman_item.idb");
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
                    standardInput.WriteLine("mxmlc -compiler.source-path=" + sourcepath + " " + maintimelinepath);
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
            Properties.Settings.Default.modlist.AddRange((from i in this.lvMods.Items.Cast<ListViewItem>()
                                                          select string.Join("|", from si in i.SubItems.Cast<ListViewItem.ListViewSubItem>()
                                                                                  select si.Text)).ToArray());
            Properties.Settings.Default.Save();
            label1.Text = "Installed Mods: " + lvMods.Items.Count.ToString();
        }

        private void loadLvItems()
        {
            if (Properties.Settings.Default.modlist == null)
            {
                Properties.Settings.Default.modlist = new StringCollection();
            }

            this.lvMods.Items.AddRange((from i in Properties.Settings.Default.modlist.Cast<string>()
                                        select new ListViewItem(i.Split('|'))).ToArray());

            label1.Text = "Installed Mods: " + lvMods.Items.Count.ToString();
        }

        private void installmod(string arg)
        {
            Clean();

            string temp = Properties.Settings.Default.datafolder + @"\temp";

            if (Directory.Exists(temp) == false)
            {
                Directory.CreateDirectory(temp);
            }

            ZipFile.ExtractToDirectory(arg, temp);

            string xmlfile = temp + "//modinfo.xml";

            if (File.Exists(xmlfile))
            {
                string modname = File.ReadLines(xmlfile).First();
                string modauthor = File.ReadAllLines(xmlfile)[1];
                var lineCount = File.ReadLines(xmlfile).Count();
                string Modid = File.ReadAllLines(xmlfile).Last();
                var files = Directory.EnumerateFiles(temp, "*.*", SearchOption.AllDirectories);


                if (lineCount == 3)
                {
                    // Added Character

                    if (Directory.Exists(Properties.Settings.Default.datafolder + @"\chara\" + Modid) == false)
                    {
                        string[] row = { modname, modauthor, "Added character" };
                        ListViewItem lvi = new ListViewItem(row);
                        lvMods.Items.Add(lvi);

                        if (Directory.Exists(Properties.Settings.Default.datafolder + @"\installed") == false)
                        {
                            Directory.CreateDirectory(Properties.Settings.Default.datafolder + @"\installed");
                            File.WriteAllLines(Properties.Settings.Default.datafolder + @"\installed\" + modname + @".xml", files);
                            File.WriteAllText(Properties.Settings.Default.datafolder + @"\installed\" + modname + " 2.xml", Modid);

                        }
                        else
                        {
                            File.WriteAllLines(Properties.Settings.Default.datafolder + @"\installed\" + modname + @".xml", files);
                            File.WriteAllText(Properties.Settings.Default.datafolder + @"\installed\" + modname + " 2.xml", Modid);
                        }

                        string text = File.ReadAllText(Properties.Settings.Default.datafolder + @"\installed\" + modname + @".xml");
                        text = text.Replace(@"\temp", "");
                        File.WriteAllText(Properties.Settings.Default.datafolder + @"\installed\" + modname + @".xml", text);

                        MoveDirectory(temp, Properties.Settings.Default.datafolder);

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
                                sw.WriteLine(@"embpack.exe CHARA01");
                            }
                        }


                        Properties.Settings.Default.addonmodlist.Add(modname);
                        Properties.Settings.Default.Save();

                        int numberid = 108 + Settings.Default.addonmodlist.Count;
                        string id = File.ReadAllLines(Properties.Settings.Default.datafolder + "//modinfo.xml").Last();

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
                                sw.WriteLine("cd " + Properties.Settings.Default.datafolder + @"\system");
                                sw.WriteLine(@"CMSXMLSerializer.exe char_model_spec.cms");
                            }
                        }
                        p.WaitForExit();

                        string cmspath = Properties.Settings.Default.datafolder + @"\system\char_model_spec.cms.xml";
                        string text2 = File.ReadAllText(cmspath);

                        text2 = text2.Replace("</CMS>", "  <Entry ID=\"" + numberid + "\" ShortName=\"" + id + "\">\r\n    <I_08 value=\"0x0\" />\r\n    <I_16 value=\"0x1\" />\r\n    <LoadCamDist value=\"0\" />\r\n    <I_22 value=\"0xc801\" />\r\n    <I_24 value=\"0xffff\" />\r\n    <I_26 value=\"0x0\" />\r\n    <I_28 value=\"0x0\" />\r\n    <BCS value=\"" + id + "\" />\r\n    <EAN value=\"" + id + "\" />\r\n    <FCE_EAN value=\"" + id + "\" />\r\n    <FCE value=\"NULL\" />\r\n    <CAM_EAN value=\"" + id + "\" />\r\n    <BAC value=\"" + id + "\" />\r\n    <BCM value=\"" + id + "\" />\r\n    <BAI value=\"" + id + "\" />\r\n    <BDM value=\"\" />\r\n  </Entry>\r\n</CMS>");

                        File.WriteAllText(cmspath, text2);

                        p.Start();

                        using (StreamWriter sw = p.StandardInput)
                        {
                            if (sw.BaseStream.CanWrite)
                            {
                                const string quote = "\"";

                                sw.WriteLine("cd " + Properties.Settings.Default.datafolder + @"\system");
                                sw.WriteLine(@"CMSXMLSerializer.exe " + quote + Properties.Settings.Default.datafolder + @"\system\char_model_spec.cms.xml" + quote);
                            }
                        }

                        p.WaitForExit();


                        p.StartInfo = info;
                        p.Start();
                        using (StreamWriter sw = p.StandardInput)
                        {
                            if (sw.BaseStream.CanWrite)
                            {
                                sw.WriteLine("cd " + Properties.Settings.Default.datafolder + @"\system");
                                sw.WriteLine(@"CSOXMLSerializer.exe chara_sound.cso");
                            }
                        }
                        p.WaitForExit();

                        string csopath = Properties.Settings.Default.datafolder + @"\system\chara_sound.cso.xml";
                        string text3 = File.ReadAllText(csopath);

                        text3 = text3.Replace("</CSO>", "  <CsoEntry Chara_ID=\"" + numberid + "\" Costume=\"0\">\r\n    <SE Path=\"\" />\r\n    <VOX Path=\"\" />\r\n    <AMK Path=\"\" />\r\n    <Skills Path=\"\" />\r\n  </CsoEntry>\r\n</CSO>");

                        File.WriteAllText(csopath, text3);

                        p.Start();

                        using (StreamWriter sw = p.StandardInput)
                        {
                            if (sw.BaseStream.CanWrite)
                            {
                                const string quote = "\"";

                                sw.WriteLine("cd " + Properties.Settings.Default.datafolder + @"\system");
                                sw.WriteLine(@"CSOXMLSerializer.exe " + quote + Properties.Settings.Default.datafolder + @"\system\chara_sound.cso.xml" + quote);
                            }
                        }

                        p.WaitForExit();


                        p.StartInfo = info;
                        p.Start();
                        using (StreamWriter sw = p.StandardInput)
                        {
                            if (sw.BaseStream.CanWrite)
                            {
                                sw.WriteLine("cd " + Properties.Settings.Default.datafolder + @"\system");
                                sw.WriteLine(@"CUSXMLSerializer.exe custom_skill.cus");
                            }
                        }
                        p.WaitForExit();

                        string cuspath = Properties.Settings.Default.datafolder + @"\system\custom_skill.cus.xml";
                        string text4 = File.ReadAllText(cuspath);

                        text4 = text4.Replace("  </Skillsets>", "    <Skillset Character_ID=\"" + numberid + "\" Costume_Index=\"0\" Model_Preset=\"0\">\r\n      <SuperSkill1 ID1=\"65535\" />\r\n      <SuperSkill2 ID1=\"65535\" />\r\n      <SuperSkill3 ID1=\"65535\" />\r\n      <SuperSkill4 ID1=\"65535\" />\r\n      <UltimateSkill1 ID1=\"65535\" />\r\n      <UltimateSkill2 ID1=\"65535\" />\r\n      <EvasiveSkill ID1=\"65535\" />\r\n      <BlastType ID1=\"65535\" />\r\n      <AwokenSkill ID1=\"0\" />\r\n    </Skillset>\r\n  </Skillsets>");
                        File.WriteAllText(cuspath, text4);

                        p.Start();

                        using (StreamWriter sw = p.StandardInput)
                        {
                            if (sw.BaseStream.CanWrite)
                            {
                                const string quote = "\"";

                                sw.WriteLine("cd " + Properties.Settings.Default.datafolder + @"\system");
                                sw.WriteLine(@"CUSXMLSerializer.exe " + quote + Properties.Settings.Default.datafolder + @"\system\custom_skill.cus.xml" + quote);
                            }
                        }

                        p.WaitForExit();



                        p.StartInfo = info;
                        p.Start();
                        using (StreamWriter sw = p.StandardInput)
                        {
                            if (sw.BaseStream.CanWrite)
                            {
                                sw.WriteLine("cd " + Properties.Settings.Default.datafolder + @"\system");
                                sw.WriteLine(@"AURXMLSerializer.exe aura_setting.aur");
                            }
                        }
                        p.WaitForExit();

                        string aurpath = Properties.Settings.Default.datafolder + @"\system\aura_setting.aur.xml";
                        string text5 = File.ReadAllText(aurpath);

                        text5 = text5.Replace("  </CharacterAuras>", "    <CharacterAura Chara_ID=\"" + numberid + "\" Costume=\"0\" Aura_ID=\"0\" Glare=\"False\" />\r\n  </CharacterAuras>");
                        File.WriteAllText(aurpath, text5);

                        p.Start();

                        using (StreamWriter sw = p.StandardInput)
                        {
                            if (sw.BaseStream.CanWrite)
                            {
                                const string quote = "\"";

                                sw.WriteLine("cd " + Properties.Settings.Default.datafolder + @"\system");
                                sw.WriteLine(@"AURXMLSerializer.exe " + quote + Properties.Settings.Default.datafolder + @"\system\aura_setting.aur.xml" + quote);
                            }
                        }

                        p.WaitForExit();


                        p.StartInfo = info;
                        p.Start();
                        using (StreamWriter sw = p.StandardInput)
                        {
                            if (sw.BaseStream.CanWrite)
                            {
                                sw.WriteLine("cd " + Properties.Settings.Default.datafolder + @"\system");
                                sw.WriteLine(@"PSCXMLSerializer.exe parameter_spec_char.psc");
                            }
                        }
                        p.WaitForExit();

                        string pscpath = Properties.Settings.Default.datafolder + @"\system\parameter_spec_char.psc.xml";
                        string text6 = File.ReadAllText(pscpath);

                        text6 = text6.Replace("  </Configuration>\r\n</PSC>", "    <PSC_Entry Chara_ID=\"" + numberid + "\">\r\n      <PscSpecEntry Costume=\"0\" Preset=\"0\">\r\n        <Camera_Position value=\"1\" />\r\n        <I_12 value=\"5\" />\r\n        <Health value=\"1.1155\" />\r\n        <F_20 value=\"1.0\" />\r\n        <Ki value=\"1.0\" />\r\n        <Ki_Recharge value=\"1.0\" />\r\n        <I_32 value=\"1\" />\r\n        <I_36 value=\"1\" />\r\n        <I_40 value=\"0\" />\r\n        <Stamina value=\"1.5\" />\r\n        <Stamina_Recharge value=\"0.75\" />\r\n        <F_52 value=\"1.0\" />\r\n        <F_56 value=\"1.1\" />\r\n        <I_60 value=\"0\" />\r\n        <Basic_Atk_Defense value=\"1.0\" />\r\n        <Basic_Ki_Defense value=\"0.95\" />\r\n        <Strike_Atk_Defense value=\"1.1\" />\r\n        <Super_Ki_Defense value=\"0.95\" />\r\n        <Ground_Speed value=\"1.0\" />\r\n        <Air_Speed value=\"1.0\" />\r\n        <Boost_Speed value=\"1.0\" />\r\n        <Dash_Speed value=\"1.0\" />\r\n        <F_96 value=\"1.0\" />\r\n        <Reinforcement_Skill_Duration value=\"1.0\" />\r\n        <F_104 value=\"1.0\" />\r\n        <Revival_HP_Amount value=\"1.0\" />\r\n        <Reviving_Speed value=\"1.0\" />\r\n        <F_116 value=\"1.0\" />\r\n        <F_120 value=\"0.55\" />\r\n        <F_124 value=\"1.0\" />\r\n        <F_128 value=\"1.0\" />\r\n        <F_132 value=\"1.0\" />\r\n        <F_136 value=\"1.0\" />\r\n        <I_140 value=\"0\" />\r\n        <F_144 value=\"1.0\" />\r\n        <F_148 value=\"1.0\" />\r\n        <F_152 value=\"1.0\" />\r\n        <F_156 value=\"1.0\" />\r\n        <F_160 value=\"1.0\" />\r\n        <F_164 value=\"1.0\" />\r\n        <Z-Soul value=\"98\" />\r\n        <I_172 value=\"1\" />\r\n        <I_176 value=\"1\" />\r\n        <F_180 value=\"8.0\" />\r\n      </PscSpecEntry>\r\n    </PSC_Entry>\r\n  </Configuration>\r\n</PSC>");
                        File.WriteAllText(pscpath, text6);

                        p.Start();

                        using (StreamWriter sw = p.StandardInput)
                        {
                            if (sw.BaseStream.CanWrite)
                            {
                                const string quote = "\"";

                                sw.WriteLine("cd " + Properties.Settings.Default.datafolder + @"\system");
                                sw.WriteLine(@"PSCXMLSerializer.exe " + quote + Properties.Settings.Default.datafolder + @"\system\parameter_spec_char.psc.xml" + quote);
                            }
                        }

                        p.WaitForExit();

                        string Charalist = Properties.Settings.Default.datafolder + @"\scripts\action_script\Charalist.as";

                        var text10 = new StringBuilder();

                        foreach (string s in File.ReadAllLines(Charalist))
                        {
                            text10.AppendLine(s.Replace("[[\"JCO\",0,0,0,[110,111]]]", "[[\"JCO\",0,0,0,[110,111]]],[[\"" + id + "\",0,0,0,[-1,-1]]]"));
                        }

                        using (var file = new StreamWriter(File.Create(Charalist)))
                        {
                            file.Write(text10.ToString());
                        }

                        CompileScripts();

                        // Old code
                        //
                        //string qxd = Properties.Settings.Default.datafolder + @"\quest\TMQ\tmq_data.qxd";
                        //ReplaceTextInFile(qxd, "XXX", id);
                        //
                        // Not needed anymore as we now know how to unlock characters and their variations via iggy editing


                        msgData[] expand = new msgData[file.data.Length + 1];
                        Array.Copy(file.data, expand, file.data.Length);
                        string nameid = file.data[file.data.Length - 1].NameID;
                        int endid = int.Parse(nameid.Substring(nameid.Length - 3, 3));
                        expand[expand.Length - 1].ID = file.data.Length;
                        expand[expand.Length - 1].Lines = new string[] { modname };
                        expand[expand.Length - 1].NameID = "chara_" + id + "_" + (endid).ToString("000");

                        file.data = expand;

                        cbList.Items.Clear();
                        for (int i = 0; i < file.data.Length; i++)
                            cbList.Items.Add(file.data[i].ID.ToString() + "-" + file.data[i].NameID);

                        msgStream.Save(file, MSGFileName);
                    }
                    else
                    {
                        Clean();
                        MessageBox.Show("A Mod with that character id is already installed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (lineCount == 2)
                {
                    // Replacer

                    if (Directory.Exists(Properties.Settings.Default.datafolder + @"\installed") == false)
                    {
                        Directory.CreateDirectory(Properties.Settings.Default.datafolder + @"\installed");
                        File.WriteAllLines(Properties.Settings.Default.datafolder + @"\installed\" + modname + @".xml", files);
                    }
                    else
                    {
                        File.WriteAllLines(Properties.Settings.Default.datafolder + @"\installed\" + modname + @".xml", files);
                    }

                    string text = File.ReadAllText(Properties.Settings.Default.datafolder + @"\installed\" + modname + @".xml");
                    text = text.Replace(@"\temp", "");
                    string txt = text;
                    string[] lines = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                    string[] row = { modname, modauthor, "Replacer" };
                    ListViewItem lvi = new ListViewItem(row);
                    if (lvMods.Items.Contains(lvi) == false)
                    {
                        foreach (string line in lines)
                        {
                            if (File.Exists(line))
                            {
                                if (MessageBox.Show("A mod containing file \"" + line + "\" is already installed, do you want to replace that file with the new one? \n\nWARNING: THIS COULD CORRUPT YOUR MODS INSTALLATION, ALWAYS KNOW WHAT YOU'RE DOING WHEN REPLACING STUFF", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                                {

                                }
                                else
                                {
                                    if (File.Exists(Properties.Settings.Default.datafolder + @"\installed\" + modname + @".xml"))
                                        File.Delete(Properties.Settings.Default.datafolder + @"\installed\" + modname + @".xml");
                                    Clean();
                                    return;
                                }
                            }
                        }

                        lvMods.Items.Add(lvi);

                        string text2 = File.ReadAllText(Properties.Settings.Default.datafolder + @"\installed\" + modname + @".xml");
                        text2 = text2.Replace(@"\temp", "");
                        File.WriteAllText(Properties.Settings.Default.datafolder + @"\installed\" + modname + @".xml", text2);

                        MoveDirectory(temp, Properties.Settings.Default.datafolder);

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
                                sw.WriteLine(@"embpack.exe CHARA01");
                            }
                        }
                        Clean();
                    }
                    else
                    {
                        Clean();
                        MessageBox.Show("A Mod with that name is already installed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                else if (lineCount == 4)
                {
                    // Added Skill

                    if (Directory.Exists(Properties.Settings.Default.datafolder + @"\installed") == false)
                    {
                        Directory.CreateDirectory(Properties.Settings.Default.datafolder + @"\installed");
                        File.WriteAllLines(Properties.Settings.Default.datafolder + @"\installed\" + modname + @".xml", files);
                    }
                    else
                    {
                        File.WriteAllLines(Properties.Settings.Default.datafolder + @"\installed\" + modname + @".xml", files);
                    }

                    string text = File.ReadAllText(Properties.Settings.Default.datafolder + @"\installed\" + modname + @".xml");
                    text = text.Replace(@"\temp", "");
                    string txt = text;
                    string[] lines = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                    string[] row = { modname, modauthor, "Added skill" };
                    ListViewItem lvi = new ListViewItem(row);
                    if (lvMods.Items.Contains(lvi) == false)
                    {
                        foreach (string line in lines)
                        {
                            if (File.Exists(line))
                            {
                                if (MessageBox.Show("A mod containing file \"" + line + "\" is already installed, do you want to replace that file with the new one? \n\nWARNING: THIS COULD CORRUPT YOUR MODS INSTALLATION, ALWAYS KNOW WHAT YOU'RE DOING WHEN REPLACING STUFF", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                                {

                                }
                                else
                                {
                                    if (File.Exists(Properties.Settings.Default.datafolder + @"\installed\" + modname + @".xml"))
                                        File.Delete(Properties.Settings.Default.datafolder + @"\installed\" + modname + @".xml");
                                    Clean();
                                    return;
                                }
                            }
                        }

                        lvMods.Items.Add(lvi);

                        string text2 = File.ReadAllText(Properties.Settings.Default.datafolder + @"\installed\" + modname + @".xml");
                        text2 = text2.Replace(@"\temp", "");
                        File.WriteAllText(Properties.Settings.Default.datafolder + @"\installed\" + modname + @".xml", text2);

                        MoveDirectory(temp, Properties.Settings.Default.datafolder);

                        MessageBox.Show("Skill Installed Successfully, you can now add it to the CUS file", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

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
                                sw.WriteLine("cd " + Properties.Settings.Default.datafolder + @"\system");
                                sw.WriteLine(@"CUSXMLSerializer.exe custom_skill.cus");
                            }
                        }

                        p.WaitForExit();

                        Process p2 = Process.Start(Properties.Settings.Default.datafolder + @"\system\custom_skill.cus.xml");

                        p2.WaitForExit();

                        p.Start();

                        using (StreamWriter sw = p.StandardInput)
                        {
                            if (sw.BaseStream.CanWrite)
                            {
                                const string quote = "\"";

                                sw.WriteLine("cd " + Properties.Settings.Default.datafolder + @"\system");
                                sw.WriteLine(@"CUSXMLSerializer.exe " + quote + Properties.Settings.Default.datafolder + @"\system\custom_skill.cus.xml" + quote);
                            }
                        }

                        p.WaitForExit();

                        MessageBox.Show("CUS File Compiled Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

                        if (File.Exists(Properties.Settings.Default.datafolder + @"\system\custom_skill.cus.xml"))
                        {
                            File.Delete(Properties.Settings.Default.datafolder + @"\system\custom_skill.cus.xml");
                        }

                        Clean();
                        MessageBox.Show("Installation Completed", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    else
                    {
                        Clean();
                        MessageBox.Show("A Mod with that name is already installed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            Clean();
            saveLvItems();
            MessageBox.Show("Installation Completed", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            loadFiles();
        }

        public string FindCharName(int id)
        {
            for (int n = 0; n < clist.Length; n++)
            {
                if (clist[n].ID == id)
                    return clist[n].Name;
            }

            return "Unknown Character";
        }
        private void cbCharacter_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtCMS1.Text = cmsfile.Data[cbCharacter.SelectedIndex].Paths[0];
            txtCMS2.Text = cmsfile.Data[cbCharacter.SelectedIndex].Paths[1];
            txtCMS3.Text = cmsfile.Data[cbCharacter.SelectedIndex].Paths[2];
            txtCMS4.Text = cmsfile.Data[cbCharacter.SelectedIndex].Paths[3];
            txtCMS5.Text = cmsfile.Data[cbCharacter.SelectedIndex].Paths[4];
            txtCMS6.Text = cmsfile.Data[cbCharacter.SelectedIndex].Paths[5];
            txtCMS7.Text = cmsfile.Data[cbCharacter.SelectedIndex].Paths[6];

            int index = pFile.FindCharacterIndex(cmsfile.Data[cbCharacter.SelectedIndex].ID);

            cbCostumes.Items.Clear();
            for (int i = 0; i < pFile.CharParam[index].p.Length; i++)
            {
                string name = Chartxt.Find("chara_" + cmsfile.Data[cbCharacter.SelectedIndex].ShortName + "_" + i.ToString("000"));
                if (name != "No Matching ID")
                    cbCostumes.Items.Add(i.ToString() + ". " + name);
                else
                {
                    name = Chartxt.Find("chara_" + cmsfile.Data[cbCharacter.SelectedIndex].ShortName + "_000");
                    cbCostumes.Items.Add(i.ToString() + ". " + name);
                }
            }

            index = csoFile.DataExist(cmsfile.Data[cbCharacter.SelectedIndex].ID, cbCostumes.SelectedIndex);
            CostumeIndex[1] = index;
            if (index > -1)
            {
                CSO_Data cd;
                cd = csoFile.Data[index];
                txtCSO1.Text = cd.Paths[0];
                txtCSO1.Text = cd.Paths[1];
                txtCSO1.Text = cd.Paths[2];
                txtCSO1.Text = cd.Paths[3];
            }

            cbCostumes.SelectedIndex = 0;
        }

        private void cbCostumes_SelectedIndexChanged(object sender, EventArgs e)
        {
            int index = csoFile.DataExist(cmsfile.Data[cbCharacter.SelectedIndex].ID, cbCostumes.SelectedIndex);
            CostumeIndex[1] = index;
            if (index > -1)
            {
                CSO_Data cd;
                cd = csoFile.Data[index];
                txtCSO1.Text = cd.Paths[0];
                txtCSO2.Text = cd.Paths[1];
                txtCSO3.Text = cd.Paths[2];
                txtCSO4.Text = cd.Paths[3];
            }

            index = CS.DataExist(cmsfile.Data[cbCharacter.SelectedIndex].ID, cbCostumes.SelectedIndex);
            CostumeIndex[0] = index;
            if (index > -1)
            {
                Char_Data cd = CS.Chars[index];
                SupLst1.SelectedIndex = CS.FindSuper(cd.SuperIDs[0]);
                SupLst2.SelectedIndex = CS.FindSuper(cd.SuperIDs[1]);
                SupLst3.SelectedIndex = CS.FindSuper(cd.SuperIDs[2]);
                SupLst4.SelectedIndex = CS.FindSuper(cd.SuperIDs[3]);

                UltLst1.SelectedIndex = CS.FindUltimate(cd.UltimateIDs[0]);
                UltLst2.SelectedIndex = CS.FindUltimate(cd.UltimateIDs[1]);

                EvaLst.SelectedIndex = CS.FindEvasive(cd.EvasiveID);

            }

            index = pFile.FindCharacterIndex(cmsfile.Data[cbCharacter.SelectedIndex].ID);

            if (index > -1)
            {
                for (int i = 0; i < pFile.type.Length; i++)
                {
                    PSClstData.Items[i].SubItems[1].Text = pFile.getVal(index, cbCostumes.SelectedIndex, i);

                }
            }
            else
            {
                for (int i = 0; i < pFile.type.Length; i++)
                {
                    PSClstData.Items[i].SubItems[1].Text = "0";

                }
            }
        }

        private void txtCMS1_TextChanged(object sender, EventArgs e)
        {
            cmsfile.Data[cbCharacter.SelectedIndex].Paths[0] = txtCMS1.Text;
        }

        private void txtCMS2_TextChanged(object sender, EventArgs e)
        {
            cmsfile.Data[cbCharacter.SelectedIndex].Paths[1] = txtCMS2.Text;
        }

        private void txtCMS3_TextChanged(object sender, EventArgs e)
        {
            cmsfile.Data[cbCharacter.SelectedIndex].Paths[2] = txtCMS3.Text;
        }

        private void txtCMS4_TextChanged(object sender, EventArgs e)
        {
            cmsfile.Data[cbCharacter.SelectedIndex].Paths[3] = txtCMS4.Text;
        }

        private void txtCMS5_TextChanged(object sender, EventArgs e)
        {
            cmsfile.Data[cbCharacter.SelectedIndex].Paths[4] = txtCMS5.Text;
        }

        private void txtCMS6_TextChanged(object sender, EventArgs e)
        {
            cmsfile.Data[cbCharacter.SelectedIndex].Paths[5] = txtCMS6.Text;
        }

        private void txtCMS7_TextChanged(object sender, EventArgs e)
        {
            cmsfile.Data[cbCharacter.SelectedIndex].Paths[6] = txtCMS7.Text;
        }

        private void txtCSO1_TextChanged(object sender, EventArgs e)
        {
            csoFile.Data[CostumeIndex[1]].Paths[0] = txtCSO1.Text;
        }

        private void txtCSO2_TextChanged(object sender, EventArgs e)
        {
            csoFile.Data[CostumeIndex[1]].Paths[1] = txtCSO2.Text;
        }

        private void txtCSO3_TextChanged(object sender, EventArgs e)
        {
            csoFile.Data[CostumeIndex[1]].Paths[2] = txtCSO3.Text;
        }

        private void txtCSO4_TextChanged(object sender, EventArgs e)
        {
            csoFile.Data[CostumeIndex[1]].Paths[3] = txtCSO4.Text;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            csoFile.Save();
            MessageBox.Show("CSO File Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void cbSuper1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SupLst1.SelectedIndex >= 0)
                CS.Chars[CostumeIndex[0]].SuperIDs[0] = CS.Supers[SupLst1.SelectedIndex].ID;
            else
                CS.Chars[CostumeIndex[0]].SuperIDs[0] = -1;
        }

        private void cbSuper2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SupLst2.SelectedIndex >= 0)
                CS.Chars[CostumeIndex[0]].SuperIDs[1] = CS.Supers[SupLst2.SelectedIndex].ID;
            else
                CS.Chars[CostumeIndex[0]].SuperIDs[1] = -1;
        }

        private void cbSuper3_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SupLst3.SelectedIndex >= 0)
                CS.Chars[CostumeIndex[0]].SuperIDs[2] = CS.Supers[SupLst3.SelectedIndex].ID;
            else
                CS.Chars[CostumeIndex[0]].SuperIDs[2] = -1;
        }

        private void cbSuper4_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (SupLst4.SelectedIndex >= 0)
                CS.Chars[CostumeIndex[0]].SuperIDs[3] = CS.Supers[SupLst4.SelectedIndex].ID;
            else
                CS.Chars[CostumeIndex[0]].SuperIDs[3] = -1;
        }

        private void cbUlt1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (UltLst1.SelectedIndex >= 0)
                CS.Chars[CostumeIndex[0]].UltimateIDs[0] = CS.Ultimates[UltLst1.SelectedIndex].ID;
            else
                CS.Chars[CostumeIndex[0]].UltimateIDs[0] = -1;
        }

        private void cbUlt2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (UltLst2.SelectedIndex >= 0)
                CS.Chars[CostumeIndex[0]].UltimateIDs[1] = CS.Ultimates[UltLst2.SelectedIndex].ID;
            else
                CS.Chars[CostumeIndex[0]].UltimateIDs[1] = -1;
        }

        private void cbEva_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (EvaLst.SelectedIndex >= 0)
                CS.Chars[CostumeIndex[0]].EvasiveID = CS.Evasives[EvaLst.SelectedIndex].ID;
            else
                CS.Chars[CostumeIndex[0]].EvasiveID = -1;

        }

        private void button3_Click(object sender, EventArgs e)
        {
            CS.Save();
            MessageBox.Show("CUS File Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void editCUSFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
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
                    sw.WriteLine("cd " + Properties.Settings.Default.datafolder + @"\system");
                    sw.WriteLine(@"CUSXMLSerializer.exe custom_skill.cus");
                }
            }

            p.WaitForExit();

            Process p2 = Process.Start(Properties.Settings.Default.datafolder + @"\system\custom_skill.cus.xml");

            p2.WaitForExit();

            p.Start();

            using (StreamWriter sw = p.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    const string quote = "\"";

                    sw.WriteLine("cd " + Properties.Settings.Default.datafolder + @"\system");
                    sw.WriteLine(@"CUSXMLSerializer.exe " + quote + Properties.Settings.Default.datafolder + @"\system\custom_skill.cus.xml" + quote);
                }
            }

            p.WaitForExit();

            MessageBox.Show("CUS File Compiled Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (File.Exists(Properties.Settings.Default.datafolder + @"\system\custom_skill.cus.xml"))
            {
                File.Delete(Properties.Settings.Default.datafolder + @"\system\custom_skill.cus.xml");
            }

            loadFiles();

        }

        private void lstPSC_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (PSClstData.SelectedItems.Count != 0)
            {
                PSCtxtName.Text = PSClstData.SelectedItems[0].SubItems[0].Text;
                PSCtxtVal.Text = PSClstData.SelectedItems[0].SubItems[1].Text;
            }
        }

        private void txtPSCVal_TextChanged(object sender, EventArgs e)
        {
            PSClstData.SelectedItems[0].SubItems[1].Text = PSCtxtVal.Text;
            pFile.SaveVal(pFile.FindCharacterIndex(cmsfile.Data[cbCharacter.SelectedIndex].ID), cbCostumes.SelectedIndex, PSClstData.SelectedItems[0].Index, PSCtxtVal.Text);
        }

        private void button4_Click(object sender, EventArgs e)
        {
            pFile.Save();
            MessageBox.Show("PSC File Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void editAURFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
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
                    sw.WriteLine("cd " + Properties.Settings.Default.datafolder + @"\system");
                    sw.WriteLine(@"AURXMLSerializer.exe aura_setting.aur");
                }
            }

            p.WaitForExit();

            Process p2 = Process.Start(Properties.Settings.Default.datafolder + @"\system\aura_setting.aur.xml");

            p2.WaitForExit();

            p.Start();

            using (StreamWriter sw = p.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    const string quote = "\"";

                    sw.WriteLine("cd " + Properties.Settings.Default.datafolder + @"\system");
                    sw.WriteLine(@"AURXMLSerializer.exe " + quote + Properties.Settings.Default.datafolder + @"\system\aura_setting.aur.xml" + quote);
                }
            }

            p.WaitForExit();

            MessageBox.Show("AUR File Compiled Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (File.Exists(Properties.Settings.Default.datafolder + @"\system\aura_setting.aur.xml"))
            {
                File.Delete(Properties.Settings.Default.datafolder + @"\system\aura_setting.aur.xml");
            }
            loadFiles();
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void installModToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = ".x1m files | *.x1m";
            ofd.Title = "Install Mod";
            ofd.Multiselect = true;


            if (ofd.ShowDialog() == DialogResult.OK && ofd.FileNames.Length > 0)
            {
                foreach (string file in ofd.FileNames)
                {
                    if (MessageBox.Show("Do you want to install \"" + Path.GetFileNameWithoutExtension(file) + "\" ?", "Mod Installation", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        installmod(file);
                    }
                }
            }
            else
            {
                return;
            }
        }

        public static void MoveDirectory(string source, string target)
        {
            var stack = new Stack<Folders>();
            stack.Push(new Folders(source, target));

            while (stack.Count > 0)
            {
                var folders = stack.Pop();
                Directory.CreateDirectory(folders.Target);
                foreach (var file in Directory.GetFiles(folders.Source, "*.*"))
                {
                    string targetFile = Path.Combine(folders.Target, Path.GetFileName(file));
                    if (File.Exists(targetFile)) File.Delete(targetFile);
                    File.Move(file, targetFile);
                }

                foreach (var folder in Directory.GetDirectories(folders.Source))
                {
                    stack.Push(new Folders(folder, Path.Combine(folders.Target, Path.GetFileName(folder))));
                }
            }
            Directory.Delete(source, true);
        }
        public class Folders
        {
            public string Source { get; private set; }
            public string Target { get; private set; }

            public Folders(string source, string target)
            {
                Source = source;
                Target = target;
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
            /*
            if (File.Exists(Properties.Settings.Default.datafolder + @"\quest\TMQ\tmq_data.qxd.bak"))
            {
                File.Delete(Properties.Settings.Default.datafolder + @"\quest\TMQ\tmq_data.qxd.bak");
            }
            */
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Clean();
        }

        private void editCMSFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
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
                    sw.WriteLine("cd " + Properties.Settings.Default.datafolder + @"\system");
                    sw.WriteLine(@"CMSXMLSerializer.exe char_model_spec.cms");
                }
            }

            p.WaitForExit();

            Process p2 = Process.Start(Properties.Settings.Default.datafolder + @"\system\char_model_spec.cms.xml");

            p2.WaitForExit();

            p.Start();

            using (StreamWriter sw = p.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    const string quote = "\"";

                    sw.WriteLine("cd " + Properties.Settings.Default.datafolder + @"\system");
                    sw.WriteLine(@"CMSXMLSerializer.exe " + quote + Properties.Settings.Default.datafolder + @"\system\char_model_spec.cms.xml" + quote);
                }
            }

            p.WaitForExit();

            MessageBox.Show("CMS File Compiled Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (File.Exists(Properties.Settings.Default.datafolder + @"\system\char_model_spec.cms.xml"))
            {
                File.Delete(Properties.Settings.Default.datafolder + @"\system\char_model_spec.cms.xml");
            }

            loadFiles();
        }

        private void saveCMSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cmsfile.Save();
            MessageBox.Show("CMS File Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void uninstallModToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ListView.SelectedIndexCollection indices = lvMods.SelectedIndices;
            if (indices.Count > 0)
            {
                Process p = new Process();
                ProcessStartInfo info = new ProcessStartInfo();
                info.FileName = "cmd.exe";
                info.CreateNoWindow = true;
                info.WindowStyle = ProcessWindowStyle.Hidden;
                info.RedirectStandardInput = true;
                info.UseShellExecute = false;

                if (File.Exists(Properties.Settings.Default.datafolder + @"\installed\" + lvMods.SelectedItems[0].Text + @".xml"))
                {
                    string[] lines = File.ReadAllLines(Properties.Settings.Default.datafolder + @"\installed\" + lvMods.SelectedItems[0].Text + @".xml");

                    foreach (string line in lines)
                    {
                        File.Delete(line);
                    }

                    //End
                }

                if (File.Exists(Properties.Settings.Default.datafolder + @"\installed\" + lvMods.SelectedItems[0].Text + @" 2.xml"))
                {
                    string id = File.ReadAllLines(Properties.Settings.Default.datafolder + @"\installed\" + lvMods.SelectedItems[0].Text + @" 2.xml").First();

                    string Charalist = Properties.Settings.Default.datafolder + @"\scripts\action_script\Charalist.as";

                    var text3 = new StringBuilder();

                    foreach (string s in File.ReadAllLines(Charalist))
                    {
                        text3.AppendLine(s.Replace(",[[\"" + id + "\",0,0,0,[-1,-1]]]", ""));
                    }

                    using (var file = new StreamWriter(File.Create(Charalist)))
                    {
                        file.Write(text3.ToString());
                    }
                    CompileScripts();

                    //string qxd = Properties.Settings.Default.datafolder + @"\quest\TMQ\tmq_data.qxd";
                    //ReplaceTextInFile(qxd, id, "XXX");

                }

                Properties.Settings.Default.addonmodlist.Remove(lvMods.SelectedItems.ToString());
                Properties.Settings.Default.Save();

                if (File.Exists(Properties.Settings.Default.datafolder + @"\installed\" + lvMods.SelectedItems + @".xml"))
                {
                    File.Delete(Properties.Settings.Default.datafolder + @"\installed\" + lvMods.SelectedItems + @".xml");
                }
                if (File.Exists(Properties.Settings.Default.datafolder + @"\installed\" + lvMods.SelectedItems + @" 2.xml"))
                {
                    File.Delete(Properties.Settings.Default.datafolder + @"\installed\" + lvMods.SelectedItems + @" 2.xml");
                }


                processDirectory(Properties.Settings.Default.datafolder);

                lvMods.Items.Remove(lvMods.SelectedItems[0]);
                saveLvItems();

                p.StartInfo = info;
                p.Start();

                using (StreamWriter sw = p.StandardInput)
                {
                    if (sw.BaseStream.CanWrite)
                    {
                        sw.WriteLine("cd " + Properties.Settings.Default.datafolder + @"\ui\texture");
                        sw.WriteLine(@"embpack.exe CHARA01");
                    }
                }

                MessageBox.Show("Mod uninstalled Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                loadFiles();
            }
        }

        private static void processDirectory(string startLocation)
        {
            foreach (var directory in Directory.GetDirectories(startLocation))
            {
                processDirectory(directory);
                if (Directory.GetFiles(directory).Length == 0 &&
                    Directory.GetDirectories(directory).Length == 0)
                {
                    Directory.Delete(directory, false);
                }
            }
        }

        private void clearInstallationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to clear installation?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (Directory.Exists(Properties.Settings.Default.datafolder))
                {
                    Directory.Delete(Properties.Settings.Default.datafolder, true);
                }

                if (Directory.Exists(Properties.Settings.Default.datafolder + @"\scripts"))
                {
                    Directory.Delete(Properties.Settings.Default.datafolder + @"\scripts", true);
                }

                Properties.Settings.Default.Reset();
                MessageBox.Show("Installation cleared, XVReborn will now close", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                return;
            }
        }

        void ReplaceTextInFile(string fileName, string oldText, string newText)
        {
            byte[] fileBytes = File.ReadAllBytes(fileName),
                oldBytes = Encoding.UTF8.GetBytes(oldText),
                newBytes = Encoding.UTF8.GetBytes(newText);

            int index = IndexOfBytes(fileBytes, oldBytes);

            if (index < 0)
            {
                // Text was not found
                return;
            }

            byte[] newFileBytes =
                new byte[fileBytes.Length + newBytes.Length - oldBytes.Length];

            Buffer.BlockCopy(fileBytes, 0, newFileBytes, 0, index);
            Buffer.BlockCopy(newBytes, 0, newFileBytes, index, newBytes.Length);
            Buffer.BlockCopy(fileBytes, index + oldBytes.Length,
                newFileBytes, index + newBytes.Length,
                fileBytes.Length - index - oldBytes.Length);

            File.WriteAllBytes(fileName, newFileBytes);
        }

        int IndexOfBytes(byte[] searchBuffer, byte[] bytesToFind)
        {
            for (int i = 0; i < searchBuffer.Length - bytesToFind.Length; i++)
            {
                bool success = true;

                for (int j = 0; j < bytesToFind.Length; j++)
                {
                    if (searchBuffer[i + j] != bytesToFind[j])
                    {
                        success = false;
                        break;
                    }
                }

                if (success)
                {
                    return i;
                }
            }

            return -1;
        }

        private void editCSOFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
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
                    sw.WriteLine("cd " + Properties.Settings.Default.datafolder + @"\system");
                    sw.WriteLine(@"CSOXMLSerializer.exe chara_sound.cso");
                }
            }

            p.WaitForExit();

            Process p2 = Process.Start(Properties.Settings.Default.datafolder + @"\system\chara_sound.cso.xml");

            p2.WaitForExit();

            p.Start();

            using (StreamWriter sw = p.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    const string quote = "\"";

                    sw.WriteLine("cd " + Properties.Settings.Default.datafolder + @"\system");
                    sw.WriteLine(@"CSOXMLSerializer.exe " + quote + Properties.Settings.Default.datafolder + @"\system\chara_sound.cso.xml" + quote);
                }
            }

            p.WaitForExit();

            MessageBox.Show("CSO File Compiled Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (File.Exists(Properties.Settings.Default.datafolder + @"\system\chara_sound.cso.xml"))
            {
                File.Delete(Properties.Settings.Default.datafolder + @"\system\chara_sound.cso.xml");
            }

            loadFiles();
        }

        private void editPSCFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
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
                    sw.WriteLine("cd " + Properties.Settings.Default.datafolder + @"\system");
                    sw.WriteLine(@"PSCXMLSerializer.exe parameter_spec_char.psc");
                }
            }

            p.WaitForExit();

            Process p2 = Process.Start(Properties.Settings.Default.datafolder + @"\system\parameter_spec_char.psc.xml");

            p2.WaitForExit();

            p.Start();

            using (StreamWriter sw = p.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    const string quote = "\"";

                    sw.WriteLine("cd " + Properties.Settings.Default.datafolder + @"\system");
                    sw.WriteLine(@"PSCXMLSerializer.exe " + quote + Properties.Settings.Default.datafolder + @"\system\parameter_spec_char.psc.xml" + quote);
                }
            }

            p.WaitForExit();

            MessageBox.Show("PSC File Compiled Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            if (File.Exists(Properties.Settings.Default.datafolder + @"\system\parameter_spec_char.psc.xml"))
            {
                File.Delete(Properties.Settings.Default.datafolder + @"\system\parameter_spec_char.psc.xml");
            }

            loadFiles();

        }

        private void cbList_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtName.Text = file.data[cbList.SelectedIndex].NameID;
            txtID.Text = file.data[cbList.SelectedIndex].ID.ToString();
            cbLine.Items.Clear();
            for (int i = 0; i < file.data[cbList.SelectedIndex].Lines.Length; i++)
                cbLine.Items.Add(i);

            cbLine.SelectedIndex = 0;
            txtText.Text = file.data[cbList.SelectedIndex].Lines[cbLine.SelectedIndex];
        }

        private void cbLine_SelectedIndexChanged(object sender, EventArgs e)
        {
            txtText.Text = file.data[cbList.SelectedIndex].Lines[cbLine.SelectedIndex];
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            file.data[cbList.SelectedIndex].NameID = txtName.Text;
            cbList.Items[cbList.SelectedIndex] = file.data[cbList.SelectedIndex].ID.ToString() + "-" + file.data[cbList.SelectedIndex].NameID;
        }

        private void txtText_TextChanged(object sender, EventArgs e)
        {
            file.data[cbList.SelectedIndex].Lines[cbLine.SelectedIndex] = txtText.Text;
        }

        private void toolStripMenuItem7_Click(object sender, EventArgs e)
        {
            msgData[] expand = new msgData[file.data.Length + 1];
            Array.Copy(file.data, expand, file.data.Length);
            string nameid = file.data[file.data.Length - 1].NameID;
            int endid = int.Parse(nameid.Substring(nameid.Length - 3, 3));
            expand[expand.Length - 1].ID = file.data.Length;
            expand[expand.Length - 1].Lines = new string[] { "New Entry" };
            expand[expand.Length - 1].NameID = nameid.Substring(0, nameid.Length - 3) + (endid + 1).ToString("000");

            file.data = expand;

            cbList.Items.Clear();
            for (int i = 0; i < file.data.Length; i++)
                cbList.Items.Add(file.data[i].ID.ToString() + "-" + file.data[i].NameID);
        }

        private void removeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            msgData[] reduce = new msgData[file.data.Length - 1];
            Array.Copy(file.data, reduce, file.data.Length - 1);
            file.data = reduce;

            cbList.Items.Clear();
            for (int i = 0; i < file.data.Length; i++)
                cbList.Items.Add(file.data[i].ID.ToString() + "-" + file.data[i].NameID);
        }

        private void charactersToolStripMenuItem_Click(object sender, EventArgs e)
        {

            MSGFileName = Properties.Settings.Default.datafolder + @"\msg\proper_noun_character_name_" + language + ".msg";
            file = msgStream.Load(MSGFileName);

            cbList.Items.Clear();
            for (int i = 0; i < file.data.Length; i++)
                cbList.Items.Add(file.data[i].ID.ToString() + " - " + file.data[i].NameID);

        }

        private void superInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MSGFileName = Properties.Settings.Default.datafolder + @"\msg\proper_noun_skill_spa_info_" + language + ".msg";
            file = msgStream.Load(MSGFileName);

            cbList.Items.Clear();
            for (int i = 0; i < file.data.Length; i++)
                cbList.Items.Add(file.data[i].ID.ToString() + " - " + file.data[i].NameID);
        }

        private void ultimatesInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MSGFileName = Properties.Settings.Default.datafolder + @"\msg\proper_noun_skill_ult_info_" + language + ".msg";
            file = msgStream.Load(MSGFileName);

            cbList.Items.Clear();
            for (int i = 0; i < file.data.Length; i++)
                cbList.Items.Add(file.data[i].ID.ToString() + " - " + file.data[i].NameID);
        }

        private void evasivesInfoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MSGFileName = Properties.Settings.Default.datafolder + @"\msg\proper_noun_skill_esc_info_" + language + ".msg";
            file = msgStream.Load(MSGFileName);

            cbList.Items.Clear();
            for (int i = 0; i < file.data.Length; i++)
                cbList.Items.Add(file.data[i].ID.ToString() + " - " + file.data[i].NameID);
        }

        private void supersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MSGFileName = Properties.Settings.Default.datafolder + @"\msg\proper_noun_skill_spa_name_" + language + ".msg";
            file = msgStream.Load(MSGFileName);

            cbList.Items.Clear();
            for (int i = 0; i < file.data.Length; i++)
                cbList.Items.Add(file.data[i].ID.ToString() + " - " + file.data[i].NameID);
        }

        private void ultimatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MSGFileName = Properties.Settings.Default.datafolder + @"\msg\proper_noun_skill_ult_name_" + language + ".msg";
            file = msgStream.Load(MSGFileName);

            cbList.Items.Clear();
            for (int i = 0; i < file.data.Length; i++)
                cbList.Items.Add(file.data[i].ID.ToString() + " - " + file.data[i].NameID);
        }

        private void evasivesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MSGFileName = Properties.Settings.Default.datafolder + @"\msg\proper_noun_skill_esc_name_" + language + ".msg";
            file = msgStream.Load(MSGFileName);

            cbList.Items.Clear();
            for (int i = 0; i < file.data.Length; i++)
                cbList.Items.Add(file.data[i].ID.ToString() + " - " + file.data[i].NameID);
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            msgStream.Save(file, MSGFileName);
            MessageBox.Show("MSG File Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            loadFiles();
        }

        private void txtAURID_TextChanged(object sender, EventArgs e)
        {
            int Num;
            if (int.TryParse(txtAURID.Text, out Num) && !CharLock)
                Chars[cbAURChar.SelectedIndex].ID = Num;
        }

        private void chkInf_CheckedChanged(object sender, EventArgs e)
        {
            if (!CharLock)
                Chars[cbAURChar.SelectedIndex].inf = chkInf.Checked;
        }

        private void cbChar_SelectedIndexChanged(object sender, EventArgs e)
        {
            CharLock = true;
            txtAURID.Text = Chars[cbAURChar.SelectedIndex].ID.ToString();
            chkInf.Checked = Chars[cbAURChar.SelectedIndex].inf;
            CharLock = false;
        }

        private void saveAURFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            List<byte> file = new List<byte>();
            byte[] signature = new byte[] { 0x23, 0x41, 0x55, 0x52, 0xFE, 0xFF, 0x20, 0x00 };
            byte[] Top = new byte[24];
            byte[] Aura1 = new byte[16 * Auras.Length];
            List<byte> Aura2 = new List<byte>();
            Array.Copy(BitConverter.GetBytes(Auras.Length), 0, Top, 0, 4);
            Array.Copy(BitConverter.GetBytes(32), 0, Top, 4, 4);
            for (int A = 0; A < Auras.Length; A++)
            {
                Array.Copy(BitConverter.GetBytes(A), 0, Aura1, (A * 16), 4);
                Array.Copy(BitConverter.GetBytes(Auras[A].Color.Length), 0, Aura1, (A * 16) + 8, 4);
                Array.Copy(BitConverter.GetBytes(32 + Aura1.Length + Aura2.Count), 0, Aura1, (A * 16) + 12, 4);
                for (int C = 0; C < Auras[A].Color.Length; C++)
                {
                    Aura2.AddRange(BitConverter.GetBytes(C));
                    if (Auras[A].Color[C] < 0)
                        Aura2.AddRange(new byte[] { 0xFF, 0xFF, 0xFF, 0xFF });
                    else
                        Aura2.AddRange(BitConverter.GetBytes(Auras[A].Color[C]));
                }
            }

            int length = 32 + Aura1.Length + Aura2.Count;

            Array.Copy(BitConverter.GetBytes(7), 0, Top, 8, 4);
            Array.Copy(BitConverter.GetBytes(length), 0, Top, 12, 4);
            //backup shift - 28,39,49,58,69,80,93
            Array.Copy(BitConverter.GetBytes(length + 28), 0, backup, 0, 4);
            Array.Copy(BitConverter.GetBytes(length + 39), 0, backup, 4, 4);
            Array.Copy(BitConverter.GetBytes(length + 49), 0, backup, 8, 4);
            Array.Copy(BitConverter.GetBytes(length + 58), 0, backup, 12, 4);
            Array.Copy(BitConverter.GetBytes(length + 69), 0, backup, 16, 4);
            Array.Copy(BitConverter.GetBytes(length + 80), 0, backup, 20, 4);
            Array.Copy(BitConverter.GetBytes(length + 93), 0, backup, 24, 4);

            length += backup.Length;

            byte[] filler = new byte[16 - (length % 16)];

            if (filler.Length != 16)
                length += filler.Length;

            Array.Copy(BitConverter.GetBytes(Chars.Length), 0, Top, 16, 4);
            Array.Copy(BitConverter.GetBytes(length), 0, Top, 20, 4);

            List<byte> Charbytes = new List<byte>();

            for (int C = 0; C < Chars.Length; C++)
            {
                Charbytes.AddRange(BitConverter.GetBytes(Chars[C].Name));
                Charbytes.AddRange(BitConverter.GetBytes(Chars[C].Costume));
                Charbytes.AddRange(BitConverter.GetBytes(Chars[C].ID));
                Charbytes.AddRange(BitConverter.GetBytes(Chars[C].inf));
                Charbytes.AddRange(new byte[] { 0x00, 0x00, 0x00 });
            }

            file.AddRange(signature);
            file.AddRange(Top);
            file.AddRange(Aura1);
            file.AddRange(Aura2);
            file.AddRange(backup);
            if (filler.Length != 16)
                file.AddRange(filler);
            file.AddRange(Charbytes);

            FileStream newfile = new FileStream(AURFileName, FileMode.Create);
            newfile.Write(file.ToArray(), 0, file.Count);
            newfile.Close();

            MessageBox.Show("AUR File Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void cbAuraList_SelectedIndexChanged(object sender, EventArgs e)
        {
            AuraLock = true;
            txtBStart.Text = Auras[cbAuraList.SelectedIndex].Color[0].ToString();
            txtBLoop.Text = Auras[cbAuraList.SelectedIndex].Color[1].ToString();
            txtBEnd.Text = Auras[cbAuraList.SelectedIndex].Color[2].ToString();
            txtKiCharge.Text = Auras[cbAuraList.SelectedIndex].Color[3].ToString();
            txtkiMax.Text = Auras[cbAuraList.SelectedIndex].Color[4].ToString();
            txtHenshinStart.Text = Auras[cbAuraList.SelectedIndex].Color[5].ToString();
            txtHenshinEnd.Text = Auras[cbAuraList.SelectedIndex].Color[6].ToString();
            AuraLock = false;
        }

        private void txtBStart_TextChanged(object sender, EventArgs e)
        {
            int Num;
            if (int.TryParse(txtBStart.Text, out Num) && !AuraLock)
                Auras[cbAuraList.SelectedIndex].Color[0] = Num;
        }

        private void txtBLoop_TextChanged(object sender, EventArgs e)
        {
            int Num;
            if (int.TryParse(txtBLoop.Text, out Num) && !AuraLock)
                Auras[cbAuraList.SelectedIndex].Color[1] = Num;
        }

        private void txtBEnd_TextChanged(object sender, EventArgs e)
        {
            int Num;
            if (int.TryParse(txtBEnd.Text, out Num) && !AuraLock)
                Auras[cbAuraList.SelectedIndex].Color[2] = Num;
        }

        private void txtKiCharge_TextChanged(object sender, EventArgs e)
        {
            int Num;
            if (int.TryParse(txtKiCharge.Text, out Num) && !AuraLock)
                Auras[cbAuraList.SelectedIndex].Color[3] = Num;
        }

        private void txtkiMax_TextChanged(object sender, EventArgs e)
        {
            int Num;
            if (int.TryParse(txtkiMax.Text, out Num) && !AuraLock)
                Auras[cbAuraList.SelectedIndex].Color[4] = Num;
        }

        private void txtHenshinStart_TextChanged(object sender, EventArgs e)
        {
            int Num;
            if (int.TryParse(txtHenshinStart.Text, out Num) && !AuraLock)
                Auras[cbAuraList.SelectedIndex].Color[5] = Num;
        }

        private void txtHenshinEnd_TextChanged(object sender, EventArgs e)
        {
            int Num;
            if (int.TryParse(txtHenshinEnd.Text, out Num) && !AuraLock)
                Auras[cbAuraList.SelectedIndex].Color[6] = Num;
        }

        private void addAuraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // add aura

            Aura[] Expand = new Aura[Auras.Length + 1];
            Array.Copy(Auras, Expand, Auras.Length);
            Auras = Expand;
            Auras[Auras.Length - 1].Color = new int[] { 0, 0, 0, 0, 0, 0, 0 };

            cbAuraList.Items.Clear();
            for (int A = 0; A < Auras.Length; A++)
                cbAuraList.Items.Add(A);
        }

        private void removeAuraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // remove aura

            if (cbAuraList.Items.Count > 22)
            {
                Aura[] reduce = new Aura[Auras.Length - 1];
                Array.Copy(Auras, reduce, Auras.Length - 1);
            }

            cbAuraList.Items.Clear();
            for (int A = 0; A < Auras.Length; A++)
                cbAuraList.Items.Add(A);
        }

        private void compileScriptsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CompileScripts();
        }

        private void lstvBasic_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstvBasic.SelectedItems.Count != 0 && !lockMod)
            {
                txtEditNameb.Text = lstvBasic.SelectedItems[0].SubItems[0].Text;
                txtEditValueb.Text = lstvBasic.SelectedItems[0].SubItems[1].Text;
            }
        }

        private void txtEditValueb_TextChanged(object sender, EventArgs e)
        {
            if (!lockMod)
            {
                lstvBasic.SelectedItems[0].SubItems[1].Text = txtEditValueb.Text;
                float n;
                if (float.TryParse(txtEditValueb.Text, out n))
                    Array.Copy(BitConverter.GetBytes(n), 0, Items[itemList.SelectedIndex].Data, 32 + (lstvBasic.SelectedItems[0].Index * 4), 4);
            }
        }

        private void lstvEffect1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstvEffect1.SelectedItems.Count != 0 && !lockMod)
            {
                txtEditName1.Text = lstvEffect1.SelectedItems[0].SubItems[0].Text;
                txtEditValue1.Text = lstvEffect1.SelectedItems[0].SubItems[1].Text;
            }
        }

        private void txtEditValue1_TextChanged(object sender, EventArgs e)
        {
            if (!lockMod)
            {
                lstvEffect1.SelectedItems[0].SubItems[1].Text = txtEditValue1.Text;
                float n;
                if (float.TryParse(txtEditValue1.Text, out n))
                    Array.Copy(BitConverter.GetBytes(n), 0, Items[itemList.SelectedIndex].Data, 256 + (lstvEffect1.SelectedItems[0].Index * 4), 4);
            }
        }

        private void lstvEffect2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (lstvEffect2.SelectedItems.Count != 0 && !lockMod)
            {
                txtEditName2.Text = lstvEffect2.SelectedItems[0].SubItems[0].Text;
                txtEditValue2.Text = lstvEffect2.SelectedItems[0].SubItems[1].Text;
            }
        }

        private void txtEditValue2_TextChanged(object sender, EventArgs e)
        {
            if (!lockMod)
            {
                lstvEffect2.SelectedItems[0].SubItems[1].Text = txtEditValue2.Text;
                float n;
                if (float.TryParse(txtEditValue2.Text, out n))
                    Array.Copy(BitConverter.GetBytes(n), 0, Items[itemList.SelectedIndex].Data, 480 + (lstvEffect2.SelectedItems[0].Index * 4), 4);
            }
        }

        public static void Applybyte(ref byte[] file, byte[] data, int pos, int count)
        {
            for (int i = 0; i < count; i++)
                file[pos + i] = data[i];
        }

        public void EffectData()
        {

            if (File.Exists("EffectData.xml"))
            {
                XmlDocument xd = new XmlDocument();
                xd.Load("EffectData.xml");
                eList.ConstructList(xd.SelectSingleNode("EffectData/Effects").ChildNodes);
                aList.ConstructList(xd.SelectSingleNode("EffectData/Activations").ChildNodes);
            }
            else
            {
                eList.ConstructFromUnknown(ref Items);
                aList.ConstructFromUnknown(ref Items);

                //build EFfectData
                XmlWriterSettings xmlWriterSettings = new XmlWriterSettings()
                {
                    Indent = true,
                    IndentChars = "\t",

                };

                using (XmlWriter xw = XmlWriter.Create("EffectData.xml", xmlWriterSettings))
                {
                    xw.WriteStartDocument();
                    xw.WriteStartElement("EffectData");
                    xw.WriteStartElement("Effects");
                    for (int i = 0; i < eList.effects.Length; i++)
                    {
                        xw.WriteStartElement("Item");
                        xw.WriteStartAttribute("id");
                        xw.WriteValue(eList.effects[i].ID);
                        xw.WriteEndAttribute();
                        xw.WriteStartAttribute("hex");
                        xw.WriteValue(String.Format("{0:X}", eList.effects[i].ID));
                        xw.WriteEndAttribute();
                        xw.WriteValue(eList.effects[i].Description);
                        xw.WriteEndElement();

                    }
                    xw.WriteEndElement();

                    xw.WriteStartElement("Activations");
                    for (int i = 0; i < aList.activations.Length; i++)
                    {
                        xw.WriteStartElement("Item");
                        xw.WriteStartAttribute("id");
                        xw.WriteValue(aList.activations[i].ID);
                        xw.WriteEndAttribute();
                        xw.WriteStartAttribute("hex");
                        xw.WriteValue(String.Format("{0:X}", aList.activations[i].ID));
                        xw.WriteEndAttribute();
                        xw.WriteValue(aList.activations[i].Description);
                        xw.WriteEndElement();
                    }
                    xw.WriteEndElement();

                    xw.WriteEndElement();
                    xw.WriteEndDocument();
                    xw.Close();
                }

            }

            cbEffect1.Items.Clear();
            cbEffect2.Items.Clear();
            cbActive1.Items.Clear();
            cbActive2.Items.Clear();

            for (int i = 0; i < eList.effects.Length; i++)
            {
                cbEffect1.Items.Add(eList.effects[i].ID.ToString() + "/" + String.Format("{0:X}", eList.effects[i].ID) + " " + eList.effects[i].Description);
                cbEffect2.Items.Add(eList.effects[i].ID.ToString() + "/" + String.Format("{0:X}", eList.effects[i].ID) + " " + eList.effects[i].Description);
            }

            for (int i = 0; i < aList.activations.Length; i++)
            {
                cbActive1.Items.Add(aList.activations[i].ID.ToString() + "/" + String.Format("{0:X}", aList.activations[i].ID) + " " + aList.activations[i].Description);
                cbActive2.Items.Add(aList.activations[i].ID.ToString() + "/" + String.Format("{0:X}", aList.activations[i].ID) + " " + aList.activations[i].Description);
            }


        }

        public int FindmsgIndex(ref msg msgdata, int id)
        {
            for (int i = 0; i < msgdata.data.Length; i++)
            {
                if (msgdata.data[i].ID == id)
                    return i;
            }
            return 0;
        }

        public byte[] int16byte(string text)
        {
            Int16 value;
            value = Int16.Parse(text);
            return BitConverter.GetBytes(value);
        }

        public byte[] int32byte(string text)
        {
            Int32 value;
            value = Int32.Parse(text);
            return BitConverter.GetBytes(value);
        }

        public byte[] floatbyte(string text)
        {
            float value;
            value = float.Parse(text);
            return BitConverter.GetBytes(value);
        }

        private void itemList_SelectedIndexChanged(object sender, EventArgs e)
        {

            UpdateData();

        }

        private void txtMsgName_TextChanged(object sender, EventArgs e)
        {
            if (!lockMod)
            {
                Names.data[Items[itemList.SelectedIndex].msgIndexName].Lines[0] = txtMsgName.Text;
                itemList.Items[itemList.SelectedIndex] = BitConverter.ToInt16(Items[itemList.SelectedIndex].Data, 0).ToString() + " / " + String.Format("{0:X}", BitConverter.ToInt16(Items[itemList.SelectedIndex].Data, 0)) + "-" + Names.data[Items[itemList.SelectedIndex].msgIndexName].Lines[0];
            }
        }

        private void txtMsgDesc_TextChanged(object sender, EventArgs e)
        {
            if (!lockMod)
                Descs.data[Items[itemList.SelectedIndex].msgIndexName].Lines[0] = txtMsgDesc.Text;
        }

        private void cbStar_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!lockMod)
                Array.Copy(BitConverter.GetBytes((short)(cbStar.SelectedIndex + 1)), 0, Items[itemList.SelectedIndex].Data, 2, 2);
        }

        private void txtNameID_TextChanged(object sender, EventArgs e)
        {
            if (!lockMod)
            {
                short ID;
                if (short.TryParse(txtNameID.Text, out ID))
                    Array.Copy(BitConverter.GetBytes(ID), 0, Items[itemList.SelectedIndex].Data, 4, 2);


                Items[itemList.SelectedIndex].msgIndexName = FindmsgIndex(ref Names, BitConverter.ToInt16(Items[itemList.SelectedIndex].Data, 4));
                txtMsgName.Text = Names.data[Items[itemList.SelectedIndex].msgIndexName].Lines[0];


                itemList.Items[itemList.SelectedIndex] = BitConverter.ToInt16(Items[itemList.SelectedIndex].Data, 0).ToString() + " / " + String.Format("{0:X}", BitConverter.ToInt16(Items[itemList.SelectedIndex].Data, 0)) + "-" + Names.data[Items[itemList.SelectedIndex].msgIndexName].Lines[0];
            }
        }

        private void txtDescID_TextChanged(object sender, EventArgs e)
        {
            if (!lockMod)
            {
                short ID;
                if (short.TryParse(txtDescID.Text, out ID))
                    Array.Copy(BitConverter.GetBytes(ID), 0, Items[itemList.SelectedIndex].Data, 6, 2);


                Items[itemList.SelectedIndex].msgIndexDesc = FindmsgIndex(ref Descs, BitConverter.ToInt16(Items[itemList.SelectedIndex].Data, 6));
                txtMsgDesc.Text = Descs.data[Items[itemList.SelectedIndex].msgIndexDesc].Lines[0];

            }
        }

        private void txtBuy_TextChanged(object sender, EventArgs e)
        {
            if (!lockMod)
            {
                int n;
                if (int.TryParse(txtBuy.Text, out n))
                    Array.Copy(BitConverter.GetBytes(n), 0, Items[itemList.SelectedIndex].Data, 16, 4);
            }
        }

        private void txtSell_TextChanged(object sender, EventArgs e)
        {
            if (!lockMod)
            {
                int n;
                if (int.TryParse(txtSell.Text, out n))
                    Array.Copy(BitConverter.GetBytes(n), 0, Items[itemList.SelectedIndex].Data, 20, 4);
            }
        }

        private void cbEffect1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!lockMod)
            {
                int ID = eList.effects[cbEffect1.SelectedIndex].ID;
                byte[] pass;
                if (ID == -1)
                    pass = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF };
                else
                    pass = BitConverter.GetBytes(ID);

                Array.Copy(pass, 0, Items[itemList.SelectedIndex].Data, 160, 4);
            }
        }

        private void cbActive1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!lockMod)
            {
                int ID = aList.activations[cbActive1.SelectedIndex].ID;
                byte[] pass;
                if (ID == -1)
                    pass = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF };
                else
                    pass = BitConverter.GetBytes(ID);

                Array.Copy(pass, 0, Items[itemList.SelectedIndex].Data, 164, 4);
            }
        }

        private void txtTimes1_TextChanged(object sender, EventArgs e)
        {
            if (!lockMod)
            {
                int ID;
                if (int.TryParse(txtTimes1.Text, out ID))
                {
                    byte[] pass;
                    if (ID == -1)
                        pass = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF };
                    else
                        pass = BitConverter.GetBytes(ID);

                    Array.Copy(pass, 0, Items[itemList.SelectedIndex].Data, 168, 4);
                }
            }
        }

        private void txtADelay1_TextChanged(object sender, EventArgs e)
        {
            if (!lockMod)
            {
                float ID;
                if (float.TryParse(txtADelay1.Text, out ID))
                {
                    byte[] pass;

                    pass = BitConverter.GetBytes(ID);

                    Array.Copy(pass, 0, Items[itemList.SelectedIndex].Data, 172, 4);
                }
            }
        }

        private void txtAVal1_TextChanged(object sender, EventArgs e)
        {
            if (!lockMod)
            {
                float ID;
                if (float.TryParse(txtAVal1.Text, out ID))
                {
                    byte[] pass;

                    pass = BitConverter.GetBytes(ID);

                    Array.Copy(pass, 0, Items[itemList.SelectedIndex].Data, 176, 4);
                }
            }
        }

        private void txtChance1_TextChanged(object sender, EventArgs e)
        {
            if (!lockMod)
            {
                int ID;
                if (int.TryParse(txtChance1.Text, out ID))
                {
                    byte[] pass;
                    if (ID == -1)
                        pass = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF };
                    else
                        pass = BitConverter.GetBytes(ID);

                    Array.Copy(pass, 0, Items[itemList.SelectedIndex].Data, 200, 4);
                }
            }
        }

        private void cbEffect2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!lockMod)
            {
                int ID = eList.effects[cbEffect2.SelectedIndex].ID;
                byte[] pass;
                if (ID == -1)
                    pass = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF };
                else
                    pass = BitConverter.GetBytes(ID);

                Array.Copy(pass, 0, Items[itemList.SelectedIndex].Data, 384, 4);
            }
        }

        private void cbActive2_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!lockMod)
            {
                int ID = aList.activations[cbActive2.SelectedIndex].ID;
                byte[] pass;
                if (ID == -1)
                    pass = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF };
                else
                    pass = BitConverter.GetBytes(ID);

                Array.Copy(pass, 0, Items[itemList.SelectedIndex].Data, 388, 4);
            }
        }

        private void txtTimes2_TextChanged(object sender, EventArgs e)
        {
            if (!lockMod)
            {
                int ID;
                if (int.TryParse(txtTimes2.Text, out ID))
                {
                    byte[] pass;
                    if (ID == -1)
                        pass = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF };
                    else
                        pass = BitConverter.GetBytes(ID);

                    Array.Copy(pass, 0, Items[itemList.SelectedIndex].Data, 392, 4);
                }
            }
        }

        private void txtADelay2_TextChanged(object sender, EventArgs e)
        {
            if (!lockMod)
            {
                float ID;
                if (float.TryParse(txtADelay2.Text, out ID))
                {
                    byte[] pass;

                    pass = BitConverter.GetBytes(ID);

                    Array.Copy(pass, 0, Items[itemList.SelectedIndex].Data, 396, 4);
                }
            }
        }

        private void txtAVal2_TextChanged(object sender, EventArgs e)
        {
            if (!lockMod)
            {
                float ID;
                if (float.TryParse(txtAVal2.Text, out ID))
                {
                    byte[] pass;

                    pass = BitConverter.GetBytes(ID);

                    Array.Copy(pass, 0, Items[itemList.SelectedIndex].Data, 400, 4);
                }
            }
        }

        private void txtChance2_TextChanged(object sender, EventArgs e)
        {
            if (!lockMod)
            {
                int ID;
                if (int.TryParse(txtChance2.Text, out ID))
                {
                    byte[] pass;
                    if (ID == -1)
                        pass = new byte[] { 0xFF, 0xFF, 0xFF, 0xFF };
                    else
                        pass = BitConverter.GetBytes(ID);

                    Array.Copy(pass, 0, Items[itemList.SelectedIndex].Data, 424, 4);
                }
            }
        }

        private void UpdateData()
        {
            lockMod = true;
            // msg data
            txtMsgName.Text = Names.data[Items[itemList.SelectedIndex].msgIndexName].Lines[0];
            txtMsgDesc.Text = Descs.data[Items[itemList.SelectedIndex].msgIndexDesc].Lines[0];

            // basic data
            txtID.Text = BitConverter.ToInt16(Items[itemList.SelectedIndex].Data, 0).ToString();
            cbStar.SelectedItem = BitConverter.ToInt16(Items[itemList.SelectedIndex].Data, 2) - 1;
            txtNameID.Text = BitConverter.ToInt16(Items[itemList.SelectedIndex].Data, 4).ToString();
            txtDescID.Text = BitConverter.ToInt16(Items[itemList.SelectedIndex].Data, 6).ToString();
            txtBuy.Text = BitConverter.ToInt32(Items[itemList.SelectedIndex].Data, 16).ToString();
            txtSell.Text = BitConverter.ToInt32(Items[itemList.SelectedIndex].Data, 20).ToString();
            txtModelID.Text = BitConverter.ToInt32(Items[itemList.SelectedIndex].Data, 612).ToString();
            for (int i = 0; i < lstvBasic.Items.Count; i++)
            {
                lstvBasic.Items[i].SubItems[1].Text = BitConverter.ToSingle(Items[itemList.SelectedIndex].Data, 32 + (i * 4)).ToString();
            }

            cbEffect1.SelectedItem = eList.FindIndex(BitConverter.ToInt32(Items[itemList.SelectedIndex].Data, 160));
            cbActive1.SelectedItem = aList.FindIndex(BitConverter.ToInt32(Items[itemList.SelectedIndex].Data, 164));
            txtTimes1.Text = BitConverter.ToInt32(Items[itemList.SelectedIndex].Data, 168).ToString();
            txtADelay1.Text = BitConverter.ToSingle(Items[itemList.SelectedIndex].Data, 172).ToString();
            txtAVal1.Text = BitConverter.ToSingle(Items[itemList.SelectedIndex].Data, 176).ToString();
            txtChance1.Text = BitConverter.ToInt32(Items[itemList.SelectedIndex].Data, 200).ToString();
            for (int i = 0; i < lstvEffect1.Items.Count; i++)
            {
                lstvEffect1.Items[i].SubItems[1].Text = BitConverter.ToSingle(Items[itemList.SelectedIndex].Data, 256 + (i * 4)).ToString();
            }

            cbEffect2.SelectedItem = eList.FindIndex(BitConverter.ToInt32(Items[itemList.SelectedIndex].Data, 384));
            cbActive2.SelectedItem = aList.FindIndex(BitConverter.ToInt32(Items[itemList.SelectedIndex].Data, 388));
            txtTimes2.Text = BitConverter.ToInt32(Items[itemList.SelectedIndex].Data, 392).ToString();
            txtADelay2.Text = BitConverter.ToSingle(Items[itemList.SelectedIndex].Data, 396).ToString();
            txtAVal2.Text = BitConverter.ToSingle(Items[itemList.SelectedIndex].Data, 400).ToString();
            txtChance2.Text = BitConverter.ToInt32(Items[itemList.SelectedIndex].Data, 424).ToString();

            for (int i = 0; i < lstvEffect2.Items.Count; i++)
            {
                lstvEffect2.Items[i].SubItems[1].Text = BitConverter.ToSingle(Items[itemList.SelectedIndex].Data, 480 + (i * 4)).ToString();
            }

            if (lstvBasic.SelectedItems.Count != 0)
            {
                txtEditNameb.Text = lstvBasic.SelectedItems[0].SubItems[0].Text;
                txtEditValueb.Text = lstvBasic.SelectedItems[0].SubItems[1].Text;
            }

            if (lstvEffect1.SelectedItems.Count != 0)
            {
                txtEditName1.Text = lstvEffect1.SelectedItems[0].SubItems[0].Text;
                txtEditValue1.Text = lstvEffect1.SelectedItems[0].SubItems[1].Text;
            }

            if (lstvEffect2.SelectedItems.Count != 0)
            {
                txtEditName2.Text = lstvEffect2.SelectedItems[0].SubItems[0].Text;
                txtEditValue2.Text = lstvEffect2.SelectedItems[0].SubItems[1].Text;
            }
            lockMod = false;
        }
        private void IDBaddToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //add/import Z -soul
            //load zss file
            OpenFileDialog browseFile = new OpenFileDialog();
            browseFile.Filter = "Template file (*.template)|*.template";
            browseFile.Title = "Select the template you want to use for the new item";
            if (browseFile.ShowDialog() == DialogResult.Cancel)
                return;

            byte[] zssfile = File.ReadAllBytes(browseFile.FileName);
            int nameCount = BitConverter.ToInt32(zssfile, 4);
            int DescCount = BitConverter.ToInt32(zssfile, 8);

            //expand the item array

            idbItem[] Expand = new idbItem[Items.Length + 1];
            Array.Copy(Items, Expand, Items.Length);
            Expand[Expand.Length - 1].Data = new byte[640];
            Items = Expand;
            short ID = BitConverter.ToInt16(Items[Items.Length - 2].Data, 0);
            ID++;
            Array.Copy(BitConverter.GetBytes(ID), Items[Items.Length - 1].Data, 2);

            //apply Zss data to added z-soul
            Array.Copy(zssfile, 12 + (nameCount * 2) + (DescCount * 2), Items[Items.Length - 1].Data, 2, 638);

            //expand Names msg
            byte[] pass;

            msgData[] Expand2 = new msgData[Names.data.Length + 1];
            Array.Copy(Names.data, Expand2, Names.data.Length);
            Expand2[Expand2.Length - 1].NameID = "talisman_" + Names.data.Length.ToString("000");
            Expand2[Expand2.Length - 1].ID = Names.data.Length;
            if (nameCount > 0)
            {
                pass = new byte[nameCount * 2];
                Array.Copy(zssfile, 12, pass, 0, nameCount * 2);
                Expand2[Expand2.Length - 1].Lines = new string[] { BytetoString(pass) };
            }
            else
                Expand2[Expand2.Length - 1].Lines = new string[] { "New Name Entry" };

            Array.Copy(BitConverter.GetBytes((short)Expand2[Expand2.Length - 1].ID), 0, Items[Items.Length - 1].Data, 4, 2);
            Names.data = Expand2;
            Items[Items.Length - 1].msgIndexName = FindmsgIndex(ref Names, Names.data[Names.data.Length - 1].ID);



            msgData[] Expand3 = new msgData[Descs.data.Length + 1];
            Array.Copy(Descs.data, Expand3, Descs.data.Length);
            Expand3[Expand3.Length - 1].NameID = "talisman_eff_" + Descs.data.Length.ToString("000");
            Expand3[Expand3.Length - 1].ID = Descs.data.Length;
            if (DescCount > 0)
            {
                pass = new byte[DescCount * 2];
                Array.Copy(zssfile, 12 + (nameCount * 2), pass, 0, DescCount * 2);
                Expand3[Expand3.Length - 1].Lines = new string[] { BytetoString(pass) };
            }
            else
                Expand3[Expand3.Length - 1].Lines = new string[] { "New Description Entry" };

            Array.Copy(BitConverter.GetBytes((short)Expand3[Expand3.Length - 1].ID), 0, Items[Items.Length - 1].Data, 6, 2);
            Descs.data = Expand3;
            Items[Items.Length - 1].msgIndexDesc = FindmsgIndex(ref Descs, Descs.data[Descs.data.Length - 1].ID);



            //loadzss(browseFile.FileName, Items.Length - 1);
            //itemList.SelectedIndex = itemList.Items.Count - 1;
            itemList.Items.Clear();
            for (int i = 0; i < Items.Length; i++)
            {
                itemList.Items.Add(BitConverter.ToInt16(Items[i].Data, 0).ToString() + " / " + String.Format("{0:X}", BitConverter.ToInt16(Items[i].Data, 0)) + "-" + Names.data[Items[i].msgIndexName].Lines[0]);
            }

        }

        private void IDBremoveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //remove Z-soul  
            if (Items.Length > 211)
            {
                itemList.SelectedIndex = 0;
                idbItem[] Reduce = new idbItem[Items.Length - 1];
                Array.Copy(Items, Reduce, Items.Length - 1);

                Items = Reduce;

                itemList.Items.Clear();
                for (int i = 0; i < Items.Length; i++)
                {
                    itemList.Items.Add(BitConverter.ToInt16(Items[i].Data, 0).ToString() + " / " + String.Format("{0:X}", BitConverter.ToInt16(Items[i].Data, 0)) + "-" + Names.data[Items[i].msgIndexName].Lines[0]);
                }


            }
        }

        private void nameToolStripMenuItem_Click(object sender, EventArgs e)
        {

            msgData[] Expand = new msgData[Names.data.Length + 1];
            Array.Copy(Names.data, Expand, Names.data.Length);
            Expand[Expand.Length - 1].NameID = "talisman_" + Names.data.Length.ToString("000");
            Expand[Expand.Length - 1].ID = Names.data.Length;
            Expand[Expand.Length - 1].Lines = new string[] { "New Name Entry" };
            Names.data = Expand;

            DialogResult msgbox = MessageBox.Show("Do you want to set current Z-soul to use this Name", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (msgbox == DialogResult.Yes)
                txtNameID.Text = Names.data[Names.data.Length - 1].ID.ToString();

        }

        private void descriptionToolStripMenuItem_Click(object sender, EventArgs e)
        {

            //remove msg name
            if (Items.Length > 211)
            {
                msgData[] reduce = new msgData[Names.data.Length - 1];
                Array.Copy(Names.data, reduce, Names.data.Length - 1);
                Names.data = reduce;
            }
        }

        private void nameToolStripMenuItem1_Click(object sender, EventArgs e)
        {



            msgData[] Expand = new msgData[Descs.data.Length + 1];
            Array.Copy(Descs.data, Expand, Descs.data.Length);
            Expand[Expand.Length - 1].NameID = "talisman_eff_" + Descs.data.Length.ToString("000");
            Expand[Expand.Length - 1].ID = Descs.data.Length;
            Expand[Expand.Length - 1].Lines = new string[] { "New Description Entry" };
            Descs.data = Expand;

            DialogResult msgbox = MessageBox.Show("Do you want to set current Z-soul to use this Description", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (msgbox == DialogResult.Yes)
                txtDescID.Text = Descs.data[Descs.data.Length - 1].ID.ToString();


        }

        private void descriptionToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //remove msg desc
            if (Items.Length > 211)
            {
                msgData[] reduce = new msgData[Descs.data.Length - 1];
                Array.Copy(Descs.data, reduce, Descs.data.Length - 1);
                Descs.data = reduce;
            }
        }

        private void exportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //export ZSS
            List<byte> zssfile = new List<byte>();
            zssfile.AddRange(new byte[] { 0x23, 0x5A, 0x53, 0x53 });
            zssfile.AddRange(BitConverter.GetBytes(txtMsgName.TextLength));


            zssfile.AddRange(BitConverter.GetBytes(txtMsgDesc.TextLength));

            zssfile.AddRange(CharByteArray(txtMsgName.Text));

            zssfile.AddRange(CharByteArray(txtMsgDesc.Text));

            byte[] itempass = new byte[638];
            Array.Copy(Items[itemList.SelectedIndex].Data, 2, itempass, 0, 638);
            zssfile.AddRange(itempass);

            FileStream fs = new FileStream(txtMsgName.Text + ".zss", FileMode.Create);
            fs.Write(zssfile.ToArray(), 0, zssfile.Count);
            fs.Close();
        }

        private byte[] CharByteArray(string text)
        {
            char[] chrArray = text.ToCharArray();
            List<byte> bytelist = new List<byte>();
            for (int i = 0; i < chrArray.Length; i++)
            {
                bytelist.AddRange(BitConverter.GetBytes(chrArray[i]));
            }
            return bytelist.ToArray();
        }

        private void importToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //import
            OpenFileDialog browseFile = new OpenFileDialog();
            browseFile.Filter = "Z Soul Share File (*.zss)|*.zss";
            browseFile.Title = "Browse for Z Soul Share File";
            if (browseFile.ShowDialog() == DialogResult.Cancel)
                return;

            loadzss(browseFile.FileName, itemList.SelectedIndex, 0, 0, false);

            UpdateData();

        }

        private void loadzss(string pFileName, int oItem, short nID, short dID, bool useID)
        {
            byte[] zssfile = File.ReadAllBytes(pFileName);
            int nameCount = BitConverter.ToInt32(zssfile, 4);
            int DescCount = BitConverter.ToInt32(zssfile, 8);

            Array.Copy(zssfile, 12 + (nameCount * 2) + (DescCount * 2), Items[oItem].Data, 2, 638);




            byte[] pass;
            if (nameCount > 0)
            {
                pass = new byte[nameCount * 2];
                Array.Copy(zssfile, 12, pass, 0, nameCount * 2);
                txtMsgName.Text = BytetoString(pass);
            }

            if (DescCount > 0)
            {
                pass = new byte[DescCount * 2];
                Array.Copy(zssfile, 12 + (nameCount * 2), pass, 0, DescCount * 2);
                txtMsgDesc.Text = BytetoString(pass);
            }



            //UpdateData();
        }

        private string BytetoString(byte[] bytes)
        {
            char[] chrArray = new char[bytes.Length / 2];
            for (int i = 0; i < bytes.Length / 2; i++)
                chrArray[i] = BitConverter.ToChar(bytes, i * 2);

            return new string(chrArray);
        }

        private void replaceImportToolStripMenuItem_Click(object sender, EventArgs e)
        {
            //import/replace
            OpenFileDialog browseFile = new OpenFileDialog();
            browseFile.Filter = "Template file (*.template)|*.template";
            browseFile.Title = "Browse for template file";
            if (browseFile.ShowDialog() == DialogResult.Cancel)
                return;

            byte[] zssfile = File.ReadAllBytes(browseFile.FileName);
            int nameCount = BitConverter.ToInt32(zssfile, 4);
            int DescCount = BitConverter.ToInt32(zssfile, 8);

            short nameID = BitConverter.ToInt16(Items[itemList.SelectedIndex].Data, 4);
            short DescID = BitConverter.ToInt16(Items[itemList.SelectedIndex].Data, 6);
            Array.Copy(zssfile, 12 + (nameCount * 2) + (DescCount * 2), Items[itemList.SelectedIndex].Data, 2, 638);

            Array.Copy(BitConverter.GetBytes(nameID), 0, Items[itemList.SelectedIndex].Data, 4, 2);
            Array.Copy(BitConverter.GetBytes(DescID), 0, Items[itemList.SelectedIndex].Data, 6, 2);

            byte[] pass;
            if (nameCount > 0)
            {
                pass = new byte[nameCount * 2];
                Array.Copy(zssfile, 12, pass, 0, nameCount * 2);
                txtMsgName.Text = BytetoString(pass);
            }

            if (DescCount > 0)
            {
                pass = new byte[DescCount * 2];
                Array.Copy(zssfile, 12 + (nameCount * 2), pass, 0, DescCount * 2);
                txtMsgDesc.Text = BytetoString(pass);
            }

            UpdateData();
        }

        private void exportToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            //export ZSS
            List<byte> zssfile = new List<byte>();
            zssfile.AddRange(new byte[] { 0x23, 0x5A, 0x53, 0x53 });
            zssfile.AddRange(BitConverter.GetBytes(txtMsgName.TextLength));



            zssfile.AddRange(BitConverter.GetBytes(txtMsgDesc.TextLength));


            zssfile.AddRange(CharByteArray(txtMsgName.Text));

            zssfile.AddRange(CharByteArray(txtMsgDesc.Text));

            byte[] itempass = new byte[638];
            Array.Copy(Items[itemList.SelectedIndex].Data, 2, itempass, 0, 638);
            zssfile.AddRange(itempass);

            SaveFileDialog browseFile = new SaveFileDialog();
            browseFile.Filter = "Template file (*.template)|*.template";
            browseFile.Title = "Save Template";

            if (browseFile.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = new FileStream(browseFile.FileName, FileMode.Create);

                fs.Write(zssfile.ToArray(), 0, zssfile.Count);
                fs.Close();
            }
        }

        private void loadidbfile(string arg1, string arg2)
        {

            byte[] idbfile = new byte[1];
            eList = new EffectList();
            aList = new ActivationList();


            int count = 0;

            IDBFileName = arg2;
            idbfile = File.ReadAllBytes(IDBFileName);
            count = BitConverter.ToInt32(idbfile, 8);


            FileNameMsgN = Properties.Settings.Default.datafolder + @"/msg/proper_noun_" + arg1 + "_name_" + Settings.Default.language + ".msg";
            Names = msgStream.Load(FileNameMsgN);

            FileNameMsgD = Properties.Settings.Default.datafolder + @"/msg/proper_noun_" + arg1 + "_info_" + Settings.Default.language + ".msg";
            Descs = msgStream.Load(FileNameMsgD);

            //idbItems set
            Items = new idbItem[count];
            for (int i = 0; i < Items.Length; i++)
            {
                Items[i].Data = new byte[640];
                Array.Copy(idbfile, 16 + (i * 640), Items[i].Data, 0, 640);
                Items[i].msgIndexName = FindmsgIndex(ref Names, BitConverter.ToInt16(Items[i].Data, 4));
                Items[i].msgIndexDesc = FindmsgIndex(ref Descs, BitConverter.ToInt16(Items[i].Data, 6));
            }



            itemList.Items.Clear();
            for (int i = 0; i < count; i++)
            {
                itemList.Items.Add(BitConverter.ToInt16(Items[i].Data, 0).ToString() + " / " + String.Format("{0:X}", BitConverter.ToInt16(Items[i].Data, 0)) + "-" + Names.data[Items[i].msgIndexName].Lines[0]);
            }
            EffectData();
            itemList.SelectedItem = 0;
        }

        private void talismanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadidbfile("talisman", Settings.Default.datafolder + @"/system/item/talisman_item.idb");
        }

        private void supersToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            loadidbfile("skill_spa", Settings.Default.datafolder + @"/system/item/skill_item.idb");

        }

        private void ultimatesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            loadidbfile("skill_ult", Settings.Default.datafolder + @"/system/item/skill_item.idb");

        }

        private void evasivesToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            loadidbfile("skill_esc", Settings.Default.datafolder + @"/system/item/skill_item.idb");

        }

        private void accessoriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadidbfile("accessory", Settings.Default.datafolder + @"/system/item/accessory_item.idb");

        }

        private void battleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadidbfile("battle", Settings.Default.datafolder + @"/system/item/battle_item.idb");

        }

        private void topToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadidbfile("costume", Settings.Default.datafolder + @"/system/item/costume_top_item.idb");

        }

        private void glovesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadidbfile("costume", Settings.Default.datafolder + @"/system/item/costume_gloves_item.idb");

        }

        private void bottomToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadidbfile("costume", Settings.Default.datafolder + @"/system/item/costume_bottom_item.idb");

        }

        private void shoesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadidbfile("costume", Settings.Default.datafolder + @"/system/item/costume_shoes_item.idb");

        }

        private void extraToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadidbfile("extra", Settings.Default.datafolder + @"/system/item/extra_item.idb");

        }

        private void materialToolStripMenuItem_Click(object sender, EventArgs e)
        {
            loadidbfile("material", Settings.Default.datafolder + @"/system/item/material_item.idb");

        }

        private void toolStripMenuItem9_Click(object sender, EventArgs e)
        {
            List<byte> Finalize = new List<byte>();
            Finalize.AddRange(new byte[] { 0x23, 0x49, 0x44, 0x42, 0xFE, 0xFF, 0x07, 0x00 });
            Finalize.AddRange(BitConverter.GetBytes(Items.Length));
            Finalize.AddRange(new byte[] { 0x10, 0x00, 0x00, 0x00 });

            for (int i = 0; i < Items.Length; i++)
                Finalize.AddRange(Items[i].Data);

            FileStream fs = new FileStream(IDBFileName, FileMode.Create);
            fs.Write(Finalize.ToArray(), 0, Finalize.Count);
            fs.Close();

            msgStream.Save(Names, FileNameMsgN);

            msgStream.Save(Descs, FileNameMsgD);

            MessageBox.Show("IDB File Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

        }

        private void txtModelID_TextChanged(object sender, EventArgs e)
        {

            if (!lockMod)
            {
                int n;
                if (int.TryParse(txtModelID.Text, out n))
                    Array.Copy(BitConverter.GetBytes(n), 0, Items[itemList.SelectedIndex].Data, 612, 4);
            }

        }

        private void editCSSFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Process p2 = Process.Start(Properties.Settings.Default.datafolder + @"\scripts\action_script\Charalist.as");

            p2.WaitForExit();

            CompileScripts();
            loadFiles();
        }
    }
}
