using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml;
using XVRebornReplacerEdition.Properties;

namespace XVRebornReplacerEdition
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

            if (Properties.Settings.Default.datafolder.Length == 0)
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
                if(!File.Exists(Settings.Default.datafolder + @"/../xinput1_3.dll"))
                {
                    var myAssembly = Assembly.GetExecutingAssembly();
                    var myStream = myAssembly.GetManifestResourceStream("XVRebornReplacerEdition.ZipFile_Blobs.xinput1_3.zip");
                    ZipArchive archive = new ZipArchive(myStream);
                    if (File.Exists(Settings.Default.datafolder + @"/../xinput1_3.dll"))
                        File.Delete(Settings.Default.datafolder + @"/../xinput1_3.dll");
                    archive.ExtractToDirectory(Settings.Default.datafolder + @"/../");
                }
            }
            if (Directory.Exists(Properties.Settings.Default.datafolder + @"\system") == false)
            {
                Directory.CreateDirectory(Properties.Settings.Default.datafolder + @"\system");

                var myAssembly = Assembly.GetExecutingAssembly();

                var myStream = myAssembly.GetManifestResourceStream("XVRebornReplacerEdition.ZipFile_Blobs.char_model_spec.zip");
                var myStream2 = myAssembly.GetManifestResourceStream("XVRebornReplacerEdition.ZipFile_Blobs.chara_sound.zip");
                var myStream3 = myAssembly.GetManifestResourceStream("XVRebornReplacerEdition.ZipFile_Blobs.parameter_spec_char.zip");
                var myStream4 = myAssembly.GetManifestResourceStream("XVRebornReplacerEdition.ZipFile_Blobs.aura_setting.zip");
                var myStream5 = myAssembly.GetManifestResourceStream("XVRebornReplacerEdition.ZipFile_Blobs.custom_skill.zip");
                var myStream7 = myAssembly.GetManifestResourceStream("XVRebornReplacerEdition.ZipFile_Blobs.item.zip");

                ZipArchive archive = new ZipArchive(myStream);
                ZipArchive archive2 = new ZipArchive(myStream2);
                ZipArchive archive3 = new ZipArchive(myStream3);
                ZipArchive archive4 = new ZipArchive(myStream4);
                ZipArchive archive5 = new ZipArchive(myStream5);
                ZipArchive archive7 = new ZipArchive(myStream7);

                archive.ExtractToDirectory(Path.Combine(Settings.Default.datafolder + @"\system"));
                archive2.ExtractToDirectory(Path.Combine(Settings.Default.datafolder + @"\system"));
                archive3.ExtractToDirectory(Path.Combine(Settings.Default.datafolder + @"\system"));
                archive4.ExtractToDirectory(Path.Combine(Settings.Default.datafolder + @"\system"));
                archive5.ExtractToDirectory(Path.Combine(Settings.Default.datafolder + @"\system"));
                archive7.ExtractToDirectory(Path.Combine(Settings.Default.datafolder + @"\system"));
            }


            if (Directory.Exists(Properties.Settings.Default.datafolder + @"\msg") == false)
            {
                var myAssembly = Assembly.GetExecutingAssembly();
                var myStream = myAssembly.GetManifestResourceStream("XVRebornReplacerEdition.ZipFile_Blobs.msg.zip");
                ZipArchive archive = new ZipArchive(myStream);
                archive.ExtractToDirectory(Path.Combine(Settings.Default.datafolder + @"\msg"));
            }
            if (Directory.Exists(Properties.Settings.Default.datafolder + @"\ui\texture") == false)
            {
                var myAssembly = Assembly.GetExecutingAssembly();
                var myStream = myAssembly.GetManifestResourceStream("XVRebornReplacerEdition.ZipFile_Blobs.CHARA01.zip");
                ZipArchive archive = new ZipArchive(myStream);
                archive.ExtractToDirectory(Path.Combine(Settings.Default.datafolder + @"\ui\texture"));
            }


            if (Properties.Settings.Default.modlist.Contains("System.Object"))
            {
                Properties.Settings.Default.modlist.Clear();
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
            foreach (CharacterData cd in cmsfile.Data.ToArray())
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
            CS.populateSkillData(Settings.Default.datafolder + @"/msg", Settings.Default.datafolder + @"/system/custom_skill.cus", language);

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

            //Load the default idb file
            loadidbfile("talisman", Settings.Default.datafolder + @"/system/item/talisman_item.idb");
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
        if (ofd.ShowDialog() == DialogResult.OK)
        {
            foreach (string file in ofd.FileNames)
            {
                string xmlfile = "./XVRebornTemp" + @"/xvmod.xml";

                    if (System.IO.File.Exists(Settings.Default.datafolder + @"/xvmod.xml"))
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
                            }
                        }
                        if (File.Exists(xmlfile))
                            File.Delete(xmlfile);
                    }



                    if (modtype == "REPLACER")
                    {
                        if (Directory.Exists("./XVRebornTemp/system/") || Directory.Exists("./XVRebornTemp/msg"))
                        {
                            MessageBox.Show("Mod contains files that may break your mod installation. Folders like msg or system are not allowed and used by the tool for important operations.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            MergeDirectoriesWithConfirmation("./XVRebornTemp", Settings.Default.datafolder);

                            Clean();

                            MessageBox.Show("Mod installed successfully", "Success", MessageBoxButtons.OK,
                                MessageBoxIcon.Information);

                            string[] row = { modname, modauthor, "Replacer" };
                            ListViewItem lvi = new ListViewItem(row);
                            lvMods.Items.Add(lvi);
                            saveLvItems();

                            loadFiles();
                        }
                    }
                    else
                    {
                        MessageBox.Show("Mod type not supported", "Error", MessageBoxButtons.OK,
                        MessageBoxIcon.Error);

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
                    File.Copy(sourceFile, destFile, true); // Replace the existing file.

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

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
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

        private void saveCMSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            cmsfile.Save();
            MessageBox.Show("CMS File Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void clearInstallationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to clear installation?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (Directory.Exists(Properties.Settings.Default.datafolder))
                    Directory.Delete(Properties.Settings.Default.datafolder, true);

                if (Directory.Exists("./XVRebornTemp"))
                    Directory.Delete("./XVRebornTemp", true);

                Properties.Settings.Default.Reset();
                MessageBox.Show("Installation cleared, XVReborn will now close", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                return;
            }
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
    }
}
