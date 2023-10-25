using Microsoft.VisualBasic.ApplicationServices;
using System.IO;
using System;
using System.IO.Compression;
using System.Xml;
using System.Windows.Forms;
using XVCharaCreator.Properties;
using XVReborn;

namespace XVCharaCreator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void buildXVModFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Save Mod File";
            sfd.Filter = "Xenoverse Mod Files (*.xvmod)|*.xvmod";

            if (txtName.Text.Length > 0 && txtAuthor.Text.Length > 0
                && Directory.Exists(txtFolder.Text)
                && sfd.ShowDialog() == DialogResult.OK
                && File.Exists(textBox1.Text))
            {
                // Specify the path where you want to save the XML file
                string xmlFilePath = "./XVCharaCreatorTemp/xvmod.xml";

                if (Directory.Exists("./XVCharaCreatorTemp"))
                    Directory.Delete("./XVCharaCreatorTemp", true);
                Directory.CreateDirectory("./XVCharaCreatorTemp");

                // Create an XmlWriterSettings instance for formatting the XML
                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "    ", // Use four spaces for indentation
                };

                // Create the XmlWriter and write the XML content
                using (XmlWriter writer = XmlWriter.Create(xmlFilePath, settings))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("XVMOD");
                    writer.WriteAttributeString("type", "ADDED_CHARACTER");

                    WriteElementWithValue(writer, "MOD_NAME", txtName.Text);
                    WriteElementWithValue(writer, "MOD_AUTHOR", txtAuthor.Text);
                    WriteElementWithValue(writer, "MOD_VERSION", txtVersion.Text);


                    // Let's start adding the actual character attributes (AUR, CMS, CSO, etc...)
                    WriteElementWithValue(writer, "AUR_ID", txtAuraID.Text);
                    if (cbAuraGlare.Checked)
                        WriteElementWithValue(writer, "AUR_GLARE", "1");
                    else
                        WriteElementWithValue(writer, "AUR_GLARE", "0");

                    WriteElementWithValue(writer, "CMS_BCS", txtBCS.Text);
                    WriteElementWithValue(writer, "CMS_EAN", txtEAN.Text);
                    WriteElementWithValue(writer, "CMS_FCE_EAN", txtFCEEAN.Text);
                    WriteElementWithValue(writer, "CMS_CAM_EAN", txtCAMEAN.Text);
                    WriteElementWithValue(writer, "CMS_BAC", txtBAC.Text);
                    WriteElementWithValue(writer, "CMS_BCM", txtBCM.Text);
                    WriteElementWithValue(writer, "CMS_BAI", txtBAI.Text);

                    WriteElementWithValue(writer, "CSO_1", txtCSO1.Text);
                    WriteElementWithValue(writer, "CSO_2", txtCSO2.Text);
                    WriteElementWithValue(writer, "CSO_3", txtCSO3.Text);
                    WriteElementWithValue(writer, "CSO_4", txtCSO4.Text);


                    SetComboBoxValue(writer, cbSuper1, "CUS_SUPER_1");
                    SetComboBoxValue(writer, cbSuper2, "CUS_SUPER_2");
                    SetComboBoxValue(writer, cbSuper3, "CUS_SUPER_3");
                    SetComboBoxValue(writer, cbSuper4, "CUS_SUPER_4");
                    SetComboBoxValue(writer, cbUltimate1, "CUS_ULTIMATE_1");
                    SetComboBoxValue(writer, cbUltimate2, "CUS_ULTIMATE_2");
                    SetComboBoxValue(writer, cbEvasive, "CUS_EVASIVE");

                    WriteElementWithValue(writer, "MSG_CHARACTER_NAME", txtMSG1.Text);
                    WriteElementWithValue(writer, "MSG_COSTUME_NAME", txtMSG2.Text);

                    WriteElementWithValue(writer, "VOX_1", txtVOX1.Text);
                    WriteElementWithValue(writer, "VOX_2", txtVOX2.Text);

                    writer.WriteEndElement(); // Close XVMOD
                    writer.WriteEndDocument(); // Close the document
                }
                Directory.CreateDirectory("./XVCharaCreatorTemp/chara/");
                Directory.Move(txtFolder.Text, @"./XVCharaCreatorTemp/chara/" + txtCharID.Text);
                Directory.CreateDirectory("./XVCharaCreatorTemp/ui/texture/CHARA01");
                File.Move(textBox1.Text, @"./XVCharaCreatorTemp/ui/texture/CHARA01/" + txtCharID.Text + "_000.DDS");

                ZipFile.CreateFromDirectory(@"./XVCharaCreatorTemp/", sfd.FileName);

                if (File.Exists(xmlFilePath))
                    File.Delete(xmlFilePath);

                MessageBox.Show("Mod Created Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Directory.Delete("./XVCharaCreatorTemp", true);

            }

        }
        // Helper method to write an element with a value
        static void WriteElementWithValue(XmlWriter writer, string elementName, string value)
        {
            writer.WriteStartElement(elementName);
            writer.WriteAttributeString("value", value);
            writer.WriteEndElement();
        }
        private void SetComboBoxValue(XmlWriter xmlWriter, ComboBox comboBox, string elementName)
        {
            if (comboBox.SelectedIndex >= 0)
                WriteElementWithValue(xmlWriter, elementName, comboBox.SelectedItem.ToString());
            else
                WriteElementWithValue(xmlWriter, elementName, "");
        }


        private void btnGenID_Click(object sender, EventArgs e)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            char[] id = new char[3];
            string generatedID;

                for (int i = 0; i < 3; i++)
                {
                    id[i] = chars[random.Next(chars.Length)];
                }

                generatedID = new string(id);
            

            txtCharID.Text = generatedID;
        }

        private void btnFolder_Click(object sender, EventArgs e)
        {
            if (txtCharID.Text.Length == 3)
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = $"Select {txtCharID.Text} Folder";
                fbd.UseDescriptionForTitle = true;

                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = fbd.SelectedPath;
                    string selectedDirName = Path.GetFileName(selectedPath);

                    if (selectedDirName == txtCharID.Text && Directory.Exists(selectedPath))
                    {
                        txtFolder.Text = selectedPath;
                    }
                    else
                    {
                        MessageBox.Show($"Please select the folder matching {txtCharID.Text}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            FolderBrowserDialog datadialog = new FolderBrowserDialog();
            datadialog.Description = "Select \"DB Xenoverse\" folder";

            if (Directory.Exists("C:\\Program Files (x86)\\Steam\\steamapps\\common\\DB Xenoverse"))
            {

                Properties.Settings.Default.data_path = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\DB Xenoverse\\data";
                Properties.Settings.Default.Save();
            }

            if (Properties.Settings.Default.data_path.Length == 0)
            {
                if (datadialog.ShowDialog() == DialogResult.OK)
                {
                    Properties.Settings.Default.data_path = datadialog.SelectedPath + @"/data";
                    Properties.Settings.Default.Save();
                }
            }

            if (!Directory.Exists(Settings.Default.data_path))
                MessageBox.Show("Data folder not found, you must start XVModManager first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            CharSkill CS = new CharSkill();
            CS.populateSkillData(Settings.Default.data_path + @"/system/custom_skill.cus");

            //populate skill lists
            foreach (skill sk in CS.Supers)
            {
                cbSuper1.Items.Add(sk.ID);
                cbSuper2.Items.Add(sk.ID);
                cbSuper3.Items.Add(sk.ID);
                cbSuper4.Items.Add(sk.ID);
            }
            foreach (skill sk in CS.Ultimates)
            {
                cbUltimate1.Items.Add(sk.ID);
                cbUltimate2.Items.Add(sk.ID);
            }
            foreach (skill sk in CS.Evasives)
            {
                cbEvasive.Items.Add(sk.ID);
            }
        }
        private void txtAuraID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)  // Allow digits and Backspace
            {
                e.Handled = true;  // Mark the event as handled, preventing non-digit input
            }
        }

        private void txtVersion_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)  // Allow digits and Backspace
            {
                e.Handled = true;  // Mark the event as handled, preventing non-digit input
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = $"Select .DDS file";
            ofd.Filter = "Direct Draw Surface files (*.DDS)|*.DDS";
            if (ofd.ShowDialog() == DialogResult.OK && File.Exists(ofd.FileName))
            {
                textBox1.Text = ofd.FileName;
            }
        }

        private void sToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}