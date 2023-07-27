﻿using FreeImageAPI;
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
using System.Threading;
using System.Windows.Forms;
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

        List<DraggableButton> buttonCharacters = new List<DraggableButton>();
        // Assuming you have a class-level variable to store the character codes and their corresponding images.
        Dictionary<string, Image> characterImages = new Dictionary<string, Image>();
        string[][][] charaList; // Class-level variable to store the parsed character data.
        Image defaultImage; // Class-level variable to store the default image.

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
            new CharName(47, "Added Character 1"),
            new CharName(48, "Eis Shenron"),
            new CharName(49, "Nuova Shenron"),
            new CharName(50, "Omega Shenron"),
            new CharName(51, "Gogeta SSJ4"),
            new CharName(52, "Hercule"),
            new CharName(53, "Demigra"),
            new CharName(55, "Added Character 2"),
            new CharName(59, "Nabana"),
            new CharName(60, "Raspberry"),
            new CharName(61, "Gohan 4 years old"),
            new CharName(62, "Mira"),
            new CharName(63, "Towa"),
            new CharName(64, "Added Character 3"),
            new CharName(65, "Whis"),
            new CharName(67, "Jaco"),
            new CharName(68, "Added Character 4"),
            new CharName(69, "Added Character 5"),
            new CharName(70, "Added Character 6"),
            new CharName(73, "Villinous Hercule"),
            new CharName(79, "Added Character 7"),
            new CharName(80, "Goku SSGSS"),
            new CharName(81, "Vegeta SSGSS"),
            new CharName(82, "Golden Frieza"),
            new CharName(83, "Added Character 8"),
            new CharName(84, "Added Character 9"),
            new CharName(85, "Added Character 10"),
            new CharName(86, "Added Character 11"),
            new CharName(87, "Added Character 12"),
            new CharName(88, "Added Character 13"),
            new CharName(95, "Added Character 14"),
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
        string FileName;
        List<string> sf = new List<string>();

        string language = "";

        public Form1()
        {
            InitializeComponent();
            LoadDefaultImage();
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
                if (!File.Exists(Path.Combine(Settings.Default.datafolder + @"/../steam_api_real.dll")))
                {
                    if (MessageBox.Show("XVPatcher or one of it's components is missing, do you want the tool to install it automatically?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                    {
                        var myAssembly = Assembly.GetExecutingAssembly();
                        var myStream = myAssembly.GetManifestResourceStream("XVReborn.ZipFile_Blobs.XVPatcher.zip");
                        ZipArchive archive = new ZipArchive(myStream);
                        if (File.Exists(Settings.Default.datafolder + @"/../steam_api.dll"))
                            File.Delete(Settings.Default.datafolder + @"/../steam_api.dll");
                        archive.ExtractToDirectory(Settings.Default.datafolder + @"/../steam_api_real.dll");
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
                var myStream7 = myAssembly.GetManifestResourceStream("XVReborn.ZipFile_Blobs.iggy_as3_test.zip");

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
                archive7.ExtractToDirectory(Path.Combine(Settings.Default.datafolder + @"\ui\iggy"));


            }

            if (Directory.Exists(Properties.Settings.Default.datafolder + @"\msg") == false)
            {
                var myAssembly = Assembly.GetExecutingAssembly();
                var myStream = myAssembly.GetManifestResourceStream("XVReborn.ZipFile_Blobs.msg.zip");
                ZipArchive archive = new ZipArchive(myStream);
                archive.ExtractToDirectory(Path.Combine(Settings.Default.datafolder + @"\msg"));
            }

            if (Directory.Exists(Properties.Settings.Default.flexsdkfolder + @"\bin\scripts") == false)
            {
                var myAssembly = Assembly.GetExecutingAssembly();
                var myStream = myAssembly.GetManifestResourceStream("XVReborn.ZipFile_Blobs.scripts.zip");
                ZipArchive archive = new ZipArchive(myStream);
                archive.ExtractToDirectory(Path.Combine(Settings.Default.flexsdkfolder + @"\bin"));
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

            FileName = Properties.Settings.Default.datafolder + @"\msg\proper_noun_character_name_" + language + ".msg";
            file = msgStream.Load(FileName);

            cbList.Items.Clear();
            for (int i = 0; i < file.data.Length; i++)
                cbList.Items.Add(file.data[i].ID.ToString() + " - " + file.data[i].NameID);

            cmsfile.Load(Properties.Settings.Default.datafolder + @"/system" + "/char_model_spec.cms");

            Chartxt = msgStream.Load(Properties.Settings.Default.datafolder + "/msg/proper_noun_character_name_" + language + ".msg");

            foreach (CMS_Data cd in cmsfile.Data)
            {
                string name = Chartxt.Find("chara_" + cd.ShortName + "_000");
                if (name == "No Matching ID")
                    cbCharacter.Items.Add("Unknown Character");
                else
                    cbCharacter.Items.Add(name);
            }

            pFile.load(Properties.Settings.Default.datafolder + @"/system" + "/parameter_spec_char.psc");

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
                    if (MessageBox.Show("Do you want to install \"" + arg + "\" ?", "Mod Installation", MessageBoxButtons.YesNo, MessageBoxIcon.Information) == DialogResult.Yes)
                    {
                        installmod(arg);
                    }
                    else
                    {
                        Clean();
                        Environment.Exit(0);
                    }
                }
            }
            flowLayoutPanelCharacters.Dock = DockStyle.Fill; // Adjust this based on your layout requirements.
            flowLayoutPanelCharacters.ControlAdded += new System.Windows.Forms.ControlEventHandler(flowLayoutPanelCharacters_ControlAdded);
            flowLayoutPanelCharacters.FlowDirection = FlowDirection.TopDown; // Set FlowDirection to TopDown.
            flowLayoutPanelCharacters.WrapContents = true; 
            flowLayoutPanelCharacters.AutoScroll = true;

            // Call the function to load character images and add them to the FlowLayoutPanel.
            LoadCharacterImages();
            AddCharacterImagesToFlowLayoutPanel();
        }


        // Event handlers for drag-and-drop reordering.
        private void ButtonCharacter_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                Button ButtonCharacter = (Button)sender;
                ButtonCharacter.DoDragDrop(ButtonCharacter, DragDropEffects.Move);
            }
        }

        private void ButtonCharacter_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move; // Set the effect to Move to allow dropping.
        }

        private void ButtonCharacter_DragDrop(object sender, DragEventArgs e)
        {
            Button targetButton = (Button)sender;
            DraggableButton sourceButton = (DraggableButton)e.Data.GetData(typeof(DraggableButton));

            int targetIndex = flowLayoutPanelCharacters.Controls.GetChildIndex(targetButton);
            int sourceIndex = flowLayoutPanelCharacters.Controls.GetChildIndex(sourceButton);

            flowLayoutPanelCharacters.Controls.SetChildIndex(sourceButton, targetIndex);

            // Update the order of DraggableButton objects in the ButtonCharacters list.
            buttonCharacters.RemoveAt(sourceIndex);
            buttonCharacters.Insert(targetIndex, sourceButton);
        }

        // Function to load the default image.
        void LoadDefaultImage()
        {
            // Replace "YourImageFolderPath" with the path to the folder containing the default image.
            string defaultImagePath = Path.Combine(Settings.Default.datafolder + @"\ui\texture\CHARA01", "FOF_000.dds");

            // Check if the default image file exists before loading.
            if (File.Exists(defaultImagePath))
            {
                try
                {
                    // Use FreeImage to load the DDS file.
                    FREE_IMAGE_FORMAT imageFormat = FREE_IMAGE_FORMAT.FIF_DDS;
                    FIBITMAP dib = FreeImage.LoadEx(defaultImagePath, ref imageFormat);
                    if (dib != null)
                    {
                        // Convert the FIBITMAP to a .NET Bitmap.
                        Bitmap defaultBitmap = FreeImage.GetBitmap(dib);

                        // Free the FIBITMAP to avoid memory leaks.
                        FreeImage.UnloadEx(ref dib);

                        // Dispose of the existing default image if it exists before adding the new one.
                        if (defaultImage != null)
                        {
                            defaultImage.Dispose();
                        }

                        // Set the default image to the loaded Bitmap.
                        defaultImage = defaultBitmap;
                    }
                    else
                    {
                    }
                }
                catch (Exception ex)
                {
                    // Handle the exception or display an error message.
                }
            }
            else
            {
                // Handle the case where the default image file is missing.
                // You may want to exit the application or handle the error differently if the default image is critical.
            }
        }
        // Function to load the character images and parse character data from Charalist.as.
        void LoadCharacterImages()
        {
            // Replace "YourImageFolderPath" with the path to the folder containing the character images.
            string imageFolderPath = Settings.Default.datafolder + @"\ui\texture\CHARA01";

            // Replace "CharaListDlc0_0" with the actual array containing the character codes.
            string[] charaListLines = File.ReadAllLines(Properties.Settings.Default.flexsdkfolder + @"/bin/scripts/action_script/Charalist.as");

            // Assuming the character data is stored in CharaListDlc0_0 variable in the AS3 file.
            string charaListVariable = "CharaListDlc0_0";
            string startToken = charaListVariable + ":Array = [";
            string endToken = "]];";
            string characterDataString = "";

            // Extract the character data from the AS3 file.
            foreach (var line in charaListLines)
            {
                if (line.Contains(startToken))
                {
                    characterDataString = line.Substring(line.IndexOf(startToken) + startToken.Length);
                    break;
                }
            }

            // Check if characterDataString is empty (not found in the AS3 file).
            if (string.IsNullOrEmpty(characterDataString))
            {
                return;
            }

            // Parse the character data string into the appropriate data structure.
            charaList = ParseCharacterData(characterDataString);

            foreach (var characterData in charaList)
            {
                string characterCode = characterData[0][0];

                DraggableButton buttonCharacter = new DraggableButton();
                buttonCharacter.BackgroundImageLayout = ImageLayout.Zoom;
                buttonCharacter.Width = 128;
                buttonCharacter.Height = 64;

                // Try to get the image from the dictionary.
                if (characterImages.TryGetValue(characterCode, out Image characterImage))
                {
                    buttonCharacter.BackgroundImage = characterImage;
                }
                else
                {
                    // If the image for the character code is not found in the dictionary,
                    // use the default image as the button's background image.
                    if (defaultImage != null)
                    {
                        buttonCharacter.BackgroundImage = new Bitmap(defaultImage);
                    }
                    else
                    {
                        // Handle the case where the default image is not available.
                        // You may want to display an error placeholder or exit the application gracefully.
                    }
                }

                // Set the Tag property of the DraggableButton to store the character code.
                buttonCharacter.Tag = characterCode;
                buttonCharacter.Text = characterCode;
                buttonCharacter.Font = new Font("Arial", 15);
                buttonCharacter.ForeColor = SystemColors.MenuText;


                // Wire up the DragEnter, DragDrop, and MouseMove events for the DraggableButton.
                buttonCharacter.DragEnter += ButtonCharacter_DragEnter;
                buttonCharacter.DragDrop += ButtonCharacter_DragDrop;
                buttonCharacter.MouseMove += ButtonCharacter_MouseMove;

                // Add the DraggableButton to the list and the FlowLayoutPanel.
                buttonCharacters.Add(buttonCharacter);
                flowLayoutPanelCharacters.Controls.Add(buttonCharacter);

                // Check if the image is not already loaded in the dictionary.
                if (!characterImages.ContainsKey(characterCode))
                {
                    // Try loading the image with "_000" suffix first.
                    string imagePathWithSuffix = Path.Combine(imageFolderPath, characterCode + "_000.dds");

                    try
                    {
                        // Use FreeImage to load the DDS file.
                        FREE_IMAGE_FORMAT imageFormat = FREE_IMAGE_FORMAT.FIF_DDS;
                        FIBITMAP dib = FreeImage.LoadEx(imagePathWithSuffix, ref imageFormat);
                        if (dib == null)
                        {
                            // If the image with suffix doesn't exist, try without the suffix.
                            string imagePathWithoutSuffix = Path.Combine(imageFolderPath, characterCode + ".dds");
                            dib = FreeImage.LoadEx(imagePathWithoutSuffix, ref imageFormat);
                        }

                        if (dib != null)
                        {
                            // Convert the FIBITMAP to a .NET Bitmap.
                            Bitmap characterBitmap = FreeImage.GetBitmap(dib);

                            // Free the FIBITMAP to avoid memory leaks.
                            FreeImage.UnloadEx(ref dib);

                            // If the image could not be loaded, use the default image.
                            if (!characterImages.ContainsKey(characterCode))
                            {
                                if (defaultImage != null)
                                {
                                    characterImages.Add(characterCode, defaultImage);
                                }
                                else
                                {
                                    // Handle the case where the default image is not available.
                                    // You may want to exit the application or handle the error differently if the default image is critical.
                                }
                            }

                            // Add the character image to the dictionary.
                            characterImages[characterCode] = characterBitmap;
                        }
                        else
                        {
                            // If both the images (with and without suffix) are not found, use the default image.
                            LoadDefaultImage();
                            if (defaultImage != null)
                            {
                                characterImages.Add(characterCode, new Bitmap(defaultImage));
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        // Handle the exception or display an error message.
                    }
                }
            }
        }

        // Function to parse the character data string into a multidimensional array.
        private string[][][] ParseCharacterData(string characterDataString)
        {
            // Check if the characterDataString is empty or null.
            if (string.IsNullOrEmpty(characterDataString))
            {
                // Return an empty array or throw an exception as appropriate.
                return new string[0][][];
            }

            // Split the characterDataString to get individual character arrays.
            string[] characterArrays = characterDataString.Split(new string[] { "],[[" }, StringSplitOptions.RemoveEmptyEntries);

            // Initialize the charaList array to hold the character data.
            string[][][] charaList = new string[characterArrays.Length][][];

            for (int i = 0; i < characterArrays.Length; i++)
            {
                // Split each character array to get individual character data.
                string[] characterData = characterArrays[i].Replace("[[", "").Replace("]]", "").Split(new string[] { "],[" }, StringSplitOptions.RemoveEmptyEntries);

                // Initialize the character data array for the current character.
                charaList[i] = new string[characterData.Length][];

                for (int j = 0; j < characterData.Length; j++)
                {
                    // Split each character's data into individual values.
                    string[] characterValues = characterData[j].Split(',');

                    // Initialize the array to store the character's values.
                    charaList[i][j] = new string[characterValues.Length];

                    for (int k = 0; k < characterValues.Length; k++)
                    {
                        // Remove unwanted characters and store the value.
                        charaList[i][j][k] = characterValues[k].Replace("[", "").Replace("]", "").Replace("\"", "").Trim();
                    }
                }
            }

            return charaList;
        }

        private void flowLayoutPanelCharacters_ControlAdded(object sender, ControlEventArgs e)
        {
            // Check if the control added is a DraggableButton.
            if (e.Control is DraggableButton buttonCharacter)
            {
                // Find the index of the button in the FlowLayoutPanel.
                int index = flowLayoutPanelCharacters.Controls.IndexOf(buttonCharacter);

                // Calculate the column index (0 to 2) of the button in the current row.
                int columnIndex = index % 3;

                // Set the flow break for every third control in each row except for the last row.
                if (columnIndex == 2 && index < flowLayoutPanelCharacters.Controls.Count - 1)
                {
                    flowLayoutPanelCharacters.SetFlowBreak(buttonCharacter, true);
                }
                else
                {
                    flowLayoutPanelCharacters.SetFlowBreak(buttonCharacter, false);
                }
            }
        }

        // Function to add images to the FlowLayoutPanel.
        void AddCharacterImagesToFlowLayoutPanel()
        {
            // Clear the FlowLayoutPanel before adding images to avoid duplicates when reloading.
            flowLayoutPanelCharacters.Controls.Clear();

            string imageFolderPath = Settings.Default.datafolder + @"\ui\texture\CHARA01";
            string defaultImagePath = Path.Combine(imageFolderPath, "FOF_000.dds");

            // Load the default image outside the loop
            LoadDefaultImage();

            foreach (var characterArray in charaList)
            {
                foreach (var characterData in characterArray)
                {
                    // The character code is the first element in the characterData array.
                    string characterCode = characterData[0].ToString();

                    // Check if the character code exists in the dictionary.
                    if (characterImages.ContainsKey(characterCode))
                    {
                        // Use the existing character image from the dictionary.
                        DraggableButton buttonCharacter = new DraggableButton();
                        buttonCharacter.BackgroundImageLayout = ImageLayout.Zoom; // Set BackgroundImageLayout to Zoom.
                        buttonCharacter.Width = 128;
                        buttonCharacter.Height = 64;
                        buttonCharacter.Image = characterImages[characterCode];

                        // Add the DraggableButton to the FlowLayoutPanel.
                        flowLayoutPanelCharacters.Controls.Add(buttonCharacter);

                        // Set the Tag property of the DraggableButton to store the character code.
                        buttonCharacter.Tag = characterCode;

                        // Wire up the DragEnter, DragDrop, and MouseMove events for the DraggableButton.
                        buttonCharacter.DragEnter += ButtonCharacter_DragEnter;
                        buttonCharacter.DragDrop += ButtonCharacter_DragDrop;
                        buttonCharacter.MouseMove += ButtonCharacter_MouseMove;
                    }
                    else
                    {
                        // Handle the case where the character image is not found.
                        // Use the default image if available.
                        if (defaultImage != null)
                        {
                            DraggableButton buttonCharacter = new DraggableButton();
                            buttonCharacter.BackgroundImageLayout = ImageLayout.Zoom; // Set BackgroundImageLayout to Zoom.
                            buttonCharacter.Width = 128;
                            buttonCharacter.Height = 64;
                            buttonCharacter.Image = new Bitmap(defaultImage);

                            // Add the DraggableButton to the FlowLayoutPanel.
                            flowLayoutPanelCharacters.Controls.Add(buttonCharacter);

                            // Set the Tag property of the DraggableButton to store the character code.
                            buttonCharacter.Tag = characterCode;
                            buttonCharacter.Text = characterCode;
                            buttonCharacter.Font = new Font("Arial", 15);
                            buttonCharacter.ForeColor = SystemColors.MenuText;

                            // Wire up the DragEnter, DragDrop, and MouseMove events for the DraggableButton.
                            buttonCharacter.DragEnter += ButtonCharacter_DragEnter;
                            buttonCharacter.DragDrop += ButtonCharacter_DragDrop;
                            buttonCharacter.MouseMove += ButtonCharacter_MouseMove;
                        }
                    }
                }
            }
        }

        // Function to handle the reordering of character slots in the FlowLayoutPanel.
        void ReorderCharacterSlots()
        {
            // Create a list to store the ordered character data.
            List<string[][]> orderedCharacterData = new List<string[][]>();

            // Create a list to keep track of the processed character codes.
            List<string> processedCharacterCodes = new List<string>();

            // Iterate through the DraggableButton controls in the FlowLayoutPanel.
            foreach (DraggableButton buttonCharacter in buttonCharacters)
            {
                // Get the character code associated with the DraggableButton.
                string characterCode = buttonCharacter.Tag.ToString();

                // Check if the character code has already been processed.
                if (!processedCharacterCodes.Contains(characterCode))
                {
                    // Find the character array in charaList with the matching character code.
                    var characterDataArray = charaList.FirstOrDefault(c => c[0][0] == characterCode);

                    if (characterDataArray != null)
                    {
                        // Add the character data array to the ordered list.
                        orderedCharacterData.Add(characterDataArray);

                        // Mark the character code as processed.
                        processedCharacterCodes.Add(characterCode);
                    }
                    else
                    {
                        // Handle the case where the character data is not found.
                        // You can display an error message or log the issue for debugging.
                    }
                }
            }

            // Convert the ordered character data list back to an array.
            charaList = orderedCharacterData.ToArray();
        }
        // Function to save the updated order in the Charalist.as file.
        void SaveCharacterOrderToFile()
        {
            string charaListFilePath = Properties.Settings.Default.flexsdkfolder + @"/bin/scripts/action_script/Charalist.as";

            // Assuming the character data is stored in CharaListDlc0_0 variable in the AS3 file.
            string charaListVariable = "public static var CharaListDlc0_0";
            string startToken = charaListVariable + ":Array = [";
            string endToken = "]]"; // Fixed the endToken to match the original array structure.
            string characterDataString = "";

            // Read the existing content of Charalist.as.
            string[] charaListLines = File.ReadAllLines(charaListFilePath);

            // Find the line that contains the character data and update it.
            for (int i = 0; i < charaListLines.Length; i++)
            {
                if (charaListLines[i].Contains(startToken))
                {
                    // Remove the old character data from the line.
                    int startIndex = charaListLines[i].IndexOf(startToken) + startToken.Length;
                    int endIndex = charaListLines[i].LastIndexOf(endToken) + endToken.Length;
                    charaListLines[i] = charaListLines[i].Remove(startIndex, endIndex - startIndex);

                    // Generate the new character data string based on the updated charaList.
                    characterDataString = GenerateCharacterDataString(charaList);

                    // Update the line with the new character data.
                    charaListLines[i] = charaListVariable + ":Array = [" + characterDataString;
                    break;
                }
            }

            // Write the updated content back to Charalist.as.
            File.WriteAllLines(charaListFilePath, charaListLines);

            // Display a message indicating successful save.
            MessageBox.Show("Character order saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Function to generate the character data string based on the updated charaList.
        private string GenerateCharacterDataString(string[][][] updatedCharaList)
        {
            // Create a new character data string based on the updated charaList.
            string newCharacterDataString = "";

            for (int i = 0; i < updatedCharaList.Length; i++)
            {
                string[][] characterArray = updatedCharaList[i];
                newCharacterDataString += "[";
                for (int j = 0; j < characterArray.Length; j++)
                {
                    var characterData = characterArray[j];

                    // The character code is the first element in the characterData array.
                    string characterCode = characterData[0];

                    // Convert the costume data and voice IDs to strings.
                    List<string> costumeDataStrings = new List<string>();
                    for (int k = 1; k < characterData.Length; k += 6)
                    {
                        string costumeData = "[\"" + characterCode + "\"," + characterData[k] + "," + characterData[k + 1] + "," +
                                             characterData[k + 2] + ",[" + characterData[k + 3] + "," +
                                             characterData[k + 4] + "]]";
                        costumeDataStrings.Add(costumeData);
                    }

                    // Join the costume data strings and append to the new character data string.
                    newCharacterDataString += string.Join(",", costumeDataStrings);

                    if (j != characterArray.Length - 1)
                    {
                        newCharacterDataString += ",";
                    }
                }

                newCharacterDataString += "]";
                if (i != updatedCharaList.Length - 1)
                {
                    newCharacterDataString += ",";
                }
            }


            newCharacterDataString += "];";
            return newCharacterDataString;
        }
        private void RemoveSemicolonBeforeSquareBracket()
        {
            string CharaList = File.ReadAllText(Properties.Settings.Default.flexsdkfolder + @"/bin/scripts/action_script/CharaList.as");

            while (CharaList.Contains(";]"))
            {
                CharaList = CharaList.Replace(";]", "]");
                Debug.WriteLine("Replaced semicolon with null");
            }

            // Now, CharaList contains the updated content with the semicolons removed.
            // You can write the updated content back to the file if needed.
            File.WriteAllText(Properties.Settings.Default.flexsdkfolder + @"/bin/scripts/action_script/CharaList.as", CharaList);
        }

        private void SaveCSSToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Reorder the character slots in the FlowLayoutPanel based on the user's arrangement.
            ReorderCharacterSlots();

            // Save the updated character order in the Charalist.as file.
            SaveCharacterOrderToFile();

            // Remove the semicolon before the square bracket
            RemoveSemicolonBeforeSquareBracket();

            // Compile Scripts
            CompileScripts();
        }

        private void CompileScripts()
        {
            ProcessStartInfo processStartInfo = new ProcessStartInfo();
            Process process = new Process();
            StringBuilder stringBuilder = new StringBuilder();
            string path3 = Settings.Default.flexsdkfolder + "\\bin\\scripts\\action_script\\CharaList.as";

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
                    standardInput.WriteLine("cd " + Settings.Default.flexsdkfolder + "\\bin");
                    standardInput.WriteLine("mxmlc -compiler.source-path=.\\\\scripts .\\\\scripts\\\\dlc3_CHARASELE_fla\\\\MainTimeline.as");
                }
            }
            process.WaitForExit();
            Directory.CreateDirectory(Settings.Default.datafolder + "\\ui\\iggy\\");

            if (File.Exists(Settings.Default.datafolder + "\\ui\\iggy\\CHARASELE.swf"))
            {
                File.Delete(Settings.Default.datafolder + "\\ui\\iggy\\CHARASELE.swf");
                if (File.Exists(Settings.Default.datafolder + "\\ui\\iggy\\CHARASELE.iggy"))
                {
                    File.Delete(Settings.Default.datafolder + "\\ui\\iggy\\CHARASELE.iggy");

                    var myAssembly = Assembly.GetExecutingAssembly();
                    var myStream = myAssembly.GetManifestResourceStream("XVReborn.ZipFile_Blobs.CHARASELE.zip");
                    ZipArchive archive = new ZipArchive(myStream);
                    archive.ExtractToDirectory(Settings.Default.datafolder + "\\ui\\iggy");

                    File.Move(Settings.Default.flexsdkfolder + "\\bin\\scripts\\dlc3_CHARASELE_fla\\MainTimeline.swf", Settings.Default.datafolder + "\\ui\\iggy\\CHARASELE.swf");
                }
                else
                {
                    var myAssembly = Assembly.GetExecutingAssembly();
                    var myStream = myAssembly.GetManifestResourceStream("XVReborn.ZipFile_Blobs.CHARASELE.zip");
                    ZipArchive archive = new ZipArchive(myStream);
                    archive.ExtractToDirectory(Settings.Default.datafolder + "\\ui\\iggy");

                    File.Move(Settings.Default.flexsdkfolder + "\\bin\\scripts\\dlc3_CHARASELE_fla\\MainTimeline.swf", Settings.Default.datafolder + "\\ui\\iggy\\CHARASELE.swf");
                }
            }
            else if (File.Exists(Settings.Default.datafolder + "\\ui\\iggy\\CHARASELE.iggy"))
            {
                File.Delete(Settings.Default.datafolder + "\\ui\\iggy\\CHARASELE.iggy");

                var myAssembly = Assembly.GetExecutingAssembly();
                var myStream = myAssembly.GetManifestResourceStream("XVReborn.ZipFile_Blobs.CHARASELE.zip");
                ZipArchive archive = new ZipArchive(myStream);
                archive.ExtractToDirectory(Settings.Default.datafolder + "\\ui\\iggy");

                File.Move(Settings.Default.flexsdkfolder + "\\bin\\scripts\\dlc3_CHARASELE_fla\\MainTimeline.swf", Settings.Default.datafolder + "\\ui\\iggy\\CHARASELE.swf");

            }
            else
            {
                var myAssembly = Assembly.GetExecutingAssembly();
                var myStream = myAssembly.GetManifestResourceStream("XVReborn.ZipFile_Blobs.CHARASELE.zip");
                ZipArchive archive = new ZipArchive(myStream);
                archive.ExtractToDirectory(Settings.Default.datafolder + "\\ui\\iggy");

                File.Move(Settings.Default.flexsdkfolder + "\\bin\\scripts\\dlc3_CHARASELE_fla\\MainTimeline.swf", Settings.Default.datafolder + "\\ui\\iggy\\CHARASELE.swf");
            }
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
            if (File.Exists(Settings.Default.datafolder + "\\ui\\iggy\\CHARASELE.swf"))
                File.Delete(Settings.Default.datafolder + "\\ui\\iggy\\CHARASELE.swf");
            if (File.Exists(Settings.Default.flexsdkfolder + "\\bin\\scripts\\dlc3_CHARASELE_fla\\MainTimeline.swf"))
                File.Delete(Settings.Default.flexsdkfolder + "\\bin\\scripts\\dlc3_CHARASELE_fla\\MainTimeline.swf");
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
                        string id = File.ReadAllLines(Properties.Settings.Default.datafolder + "//modinfo.xml").Last();

                        Properties.Settings.Default.addonmodlist.Add(modname);
                        Properties.Settings.Default.Save();

                        if (Properties.Settings.Default.addonmodlist.Count == 1)
                        {
                            text2 = text2.Replace("X01", id);
                            File.WriteAllText(cmspath, text2);
                        }

                        if (Properties.Settings.Default.addonmodlist.Count == 2)
                        {
                            text2 = text2.Replace("X02", id);
                            File.WriteAllText(cmspath, text2);
                        }

                        if (Properties.Settings.Default.addonmodlist.Count == 3)
                        {
                            text2 = text2.Replace("X03", id);
                            File.WriteAllText(cmspath, text2);
                        }

                        if (Properties.Settings.Default.addonmodlist.Count == 4)
                        {
                            text2 = text2.Replace("X04", id);
                            File.WriteAllText(cmspath, text2);
                        }

                        if (Properties.Settings.Default.addonmodlist.Count == 5)
                        {
                            text2 = text2.Replace("X05", id);
                            File.WriteAllText(cmspath, text2);
                        }

                        if (Properties.Settings.Default.addonmodlist.Count == 6)
                        {
                            text2 = text2.Replace("X06", id);
                            File.WriteAllText(cmspath, text2);
                        }

                        if (Properties.Settings.Default.addonmodlist.Count == 7)
                        {
                            text2 = text2.Replace("X07", id);
                            File.WriteAllText(cmspath, text2);
                        }

                        if (Properties.Settings.Default.addonmodlist.Count == 8)
                        {
                            text2 = text2.Replace("X08", id);
                            File.WriteAllText(cmspath, text2);
                        }

                        if (Properties.Settings.Default.addonmodlist.Count == 9)
                        {
                            text2 = text2.Replace("X09", id);
                            File.WriteAllText(cmspath, text2);
                        }

                        if (Properties.Settings.Default.addonmodlist.Count == 10)
                        {
                            text2 = text2.Replace("X10", id);
                            File.WriteAllText(cmspath, text2);
                        }

                        if (Properties.Settings.Default.addonmodlist.Count == 11)
                        {
                            text2 = text2.Replace("X11", id);
                            File.WriteAllText(cmspath, text2);
                        }

                        if (Properties.Settings.Default.addonmodlist.Count == 12)
                        {
                            text2 = text2.Replace("X12", id);
                            File.WriteAllText(cmspath, text2);
                        }

                        if (Properties.Settings.Default.addonmodlist.Count == 13)
                        {
                            text2 = text2.Replace("X13", id);
                            File.WriteAllText(cmspath, text2);
                        }
                        if (Properties.Settings.Default.addonmodlist.Count == 14)
                        {
                            text2 = text2.Replace("X14", id);
                            File.WriteAllText(cmspath, text2);
                        }

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

                        string Charalist = Properties.Settings.Default.flexsdkfolder + @"\bin\scripts\action_script\Charalist.as";

                        var text3 = new StringBuilder();

                        foreach (string s in File.ReadAllLines(Charalist))
                        {
                            text3.AppendLine(s.Replace("[[\"JCO\",0,0,0,[110,111]]]", "[[\"JCO\",0,0,0,[110,111]]],[[\"" + id + "\",0,0,0,[-1,-1]]]"));
                        }

                        CompileScripts();



                        using (var file = new StreamWriter(File.Create(Charalist)))
                        {
                            file.Write(text3.ToString());
                        }

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

                        msgStream.Save(file, FileName);
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
                                MessageBox.Show("A mod containing file \n" + line + "\n is already installed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                File.Delete(Properties.Settings.Default.datafolder + @"\installed\" + modname + @".xml");
                                Clean();
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
                                MessageBox.Show("A mod containing file \n" + line + "\n is already installed", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                File.Delete(Properties.Settings.Default.datafolder + @"\installed\" + modname + @".xml");
                                Clean();
                            }
                        }

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
            Application.Restart();
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

            Application.Restart();

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
            Application.Restart();
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
                foreach(string file in ofd.FileNames)
                {
                    installmod(file);
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

            if (File.Exists(Application.StartupPath + @"\Resources\CHARA01.emb"))
            {
                File.Delete(Application.StartupPath + @"\Resources\CHARA01.emb");
            }

            if (File.Exists(Application.StartupPath + @"\Resources\CHARAS01.emb"))
            {
                File.Delete(Application.StartupPath + @"\Resources\CHARAS01.emb");
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

            Application.Restart();
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

                    if (Properties.Settings.Default.addonmodlist.Count == 1)
                    {
                        text2 = text2.Replace(id, "X01");
                        File.WriteAllText(cmspath, text2);
                    }

                    if (Properties.Settings.Default.addonmodlist.Count == 2)
                    {
                        text2 = text2.Replace(id, "X02");
                        File.WriteAllText(cmspath, text2);
                    }

                    if (Properties.Settings.Default.addonmodlist.Count == 3)
                    {
                        text2 = text2.Replace(id, "X03");
                        File.WriteAllText(cmspath, text2);
                    }

                    if (Properties.Settings.Default.addonmodlist.Count == 4)
                    {
                        text2 = text2.Replace(id, "X04");
                        File.WriteAllText(cmspath, text2);
                    }

                    if (Properties.Settings.Default.addonmodlist.Count == 5)
                    {
                        text2 = text2.Replace(id, "X05");
                        File.WriteAllText(cmspath, text2);
                    }

                    if (Properties.Settings.Default.addonmodlist.Count == 6)
                    {
                        text2 = text2.Replace(id, "X06");
                        File.WriteAllText(cmspath, text2);
                    }

                    if (Properties.Settings.Default.addonmodlist.Count == 7)
                    {
                        text2 = text2.Replace(id, "X07");
                        File.WriteAllText(cmspath, text2);
                    }

                    if (Properties.Settings.Default.addonmodlist.Count == 8)
                    {
                        text2 = text2.Replace(id, "X08");
                        File.WriteAllText(cmspath, text2);
                    }

                    if (Properties.Settings.Default.addonmodlist.Count == 9)
                    {
                        text2 = text2.Replace(id, "X09");
                        File.WriteAllText(cmspath, text2);
                    }

                    if (Properties.Settings.Default.addonmodlist.Count == 10)
                    {
                        text2 = text2.Replace(id, "X10");
                        File.WriteAllText(cmspath, text2);
                    }

                    if (Properties.Settings.Default.addonmodlist.Count == 11)
                    {
                        text2 = text2.Replace(id, "X11");
                        File.WriteAllText(cmspath, text2);
                    }

                    if (Properties.Settings.Default.addonmodlist.Count == 12)
                    {
                        text2 = text2.Replace(id, "X12");
                        File.WriteAllText(cmspath, text2);
                    }
                    if (Properties.Settings.Default.addonmodlist.Count == 13)
                    {
                        text2 = text2.Replace(id, "X13");
                        File.WriteAllText(cmspath, text2);
                    }
                    if (Properties.Settings.Default.addonmodlist.Count == 14)
                    {
                        text2 = text2.Replace(id, "X14");
                        File.WriteAllText(cmspath, text2);
                    }

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

                    string Charalist = Properties.Settings.Default.flexsdkfolder + @"\bin\scripts\action_script\Charalist.as";

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
                Application.Restart();
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

                if (Directory.Exists(Properties.Settings.Default.flexsdkfolder + @"\bin\scripts"))
                {
                    Directory.Delete(Properties.Settings.Default.flexsdkfolder + @"\bin\scripts", true);
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

            Application.Restart();
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

            Application.Restart();

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

        private void txtID_TextChanged(object sender, EventArgs e)
        {

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

            FileName = Properties.Settings.Default.datafolder + @"\msg\proper_noun_character_name_" + language + ".msg";
            file = msgStream.Load(FileName);

            cbList.Items.Clear();
            for (int i = 0; i < file.data.Length; i++)
                cbList.Items.Add(file.data[i].ID.ToString() + " - " + file.data[i].NameID);

        }

        private void supersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileName = Properties.Settings.Default.datafolder + @"\msg\proper_noun_skill_spa_name_" + language + ".msg";
            file = msgStream.Load(FileName);

            cbList.Items.Clear();
            for (int i = 0; i < file.data.Length; i++)
                cbList.Items.Add(file.data[i].ID.ToString() + " - " + file.data[i].NameID);
        }

        private void ultimatesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileName = Properties.Settings.Default.datafolder + @"\msg\proper_noun_skill_ult_name_" + language + ".msg";
            file = msgStream.Load(FileName);

            cbList.Items.Clear();
            for (int i = 0; i < file.data.Length; i++)
                cbList.Items.Add(file.data[i].ID.ToString() + " - " + file.data[i].NameID);
        }

        private void evasivesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            FileName = Properties.Settings.Default.datafolder + @"\msg\proper_noun_skill_esc_name_" + language + ".msg";
            file = msgStream.Load(FileName);

            cbList.Items.Clear();
            for (int i = 0; i < file.data.Length; i++)
                cbList.Items.Add(file.data[i].ID.ToString() + " - " + file.data[i].NameID);
        }

        private void toolStripMenuItem3_Click(object sender, EventArgs e)
        {
            msgStream.Save(file, FileName);
            MessageBox.Show("MSG File Saved Successfully", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);

            Application.Restart();
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
    }
}
