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


                    WriteElementWithValue(writer, "CUS_SUPER_1", tbSuper1.Text);
                    WriteElementWithValue(writer, "CUS_SUPER_2", tbSuper2.Text);
                    WriteElementWithValue(writer, "CUS_SUPER_3", tbSuper3.Text);
                    WriteElementWithValue(writer, "CUS_SUPER_4", tbSuper4.Text);
                    WriteElementWithValue(writer, "CUS_ULTIMATE_1", tbUlt1.Text);
                    WriteElementWithValue(writer, "CUS_ULTIMATE_2", tbUlt2.Text);
                    WriteElementWithValue(writer, "CUS_EVASIVE", tbEva1.Text);

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
                if (Directory.Exists(txtXtraFiles.Text))
                    Directory.Move(txtXtraFiles.Text, "./XVCharaCreatorTemp/");
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

        private void button2_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = $"Select Extra Folder (Data)";
            fbd.UseDescriptionForTitle = true;

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                string selectedPath = fbd.SelectedPath;

                txtXtraFiles.Text = selectedPath;

            }
        }

    }
}