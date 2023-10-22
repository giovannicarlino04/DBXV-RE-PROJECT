using System.IO.Compression;
using System.Xml;

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
                && sfd.ShowDialog() == DialogResult.OK)
            {
                // Specify the path where you want to save the XML file
                string xmlFilePath = txtFolder.Text + "/xvmod.xml";

                if (File.Exists(xmlFilePath))
                    File.Delete(xmlFilePath);

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
                        WriteElementWithValue(writer, "AUR_GLARE", "true");
                    else
                        WriteElementWithValue(writer, "AUR_GLARE", "false");

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


                    WriteElementWithValue(writer, "CUS_SUPER_1", txtBCS.Text);
                    WriteElementWithValue(writer, "CUS_SUPER_2", txtEAN.Text);
                    WriteElementWithValue(writer, "CUS_SUPER_3", txtFCEEAN.Text);
                    WriteElementWithValue(writer, "CUS_SUPER_4", txtCAMEAN.Text);
                    WriteElementWithValue(writer, "CUS_ULTIMATE_1", txtBAC.Text);
                    WriteElementWithValue(writer, "CUS_ULTIMATE_2", txtBCM.Text);
                    WriteElementWithValue(writer, "CUS_EVASIVE_1", cbEvasive.SelectedItem.ToString());




                    writer.WriteEndElement(); // Close XVMOD
                    writer.WriteEndDocument(); // Close the document
                }

                Console.WriteLine("XML file created at: " + xmlFilePath);

                ZipFile.CreateFromDirectory(txtFolder.Text, sfd.FileName);

                if (File.Exists(xmlFilePath))
                    File.Delete(xmlFilePath);

                MessageBox.Show("Mod Created Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            // Helper method to write an element with a value
            static void WriteElementWithValue(XmlWriter writer, string elementName, string value)
            {
                writer.WriteStartElement(elementName);
                writer.WriteAttributeString("value", value);
                writer.WriteEndElement();
            }
        }

        private void btnGenID_Click(object sender, EventArgs e)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            char[] id = new char[3];
            string generatedID;

            do
            {
                for (int i = 0; i < 3; i++)
                {
                    id[i] = chars[random.Next(chars.Length)];
                }

                generatedID = new string(id);
            } while (Xenoverse.Xenoverse.characterIds.Contains(generatedID));

            txtCharID.Text = generatedID;
        }

        private void btnFolder_Click(object sender, EventArgs e)
        {
            if (txtCharID.Text.Length == 3 && !Xenoverse.Xenoverse.characterIds.Contains(txtCharID.Text))
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = $"Select {txtCharID.Text} Folder";
                fbd.UseDescriptionForTitle = true;

                if (fbd.ShowDialog() == DialogResult.OK &&
                    Path.GetDirectoryName(fbd.SelectedPath) == txtCharID.Text
                    && Directory.Exists(fbd.SelectedPath))
                    txtFolder.Text = fbd.SelectedPath;
                else if (Path.GetDirectoryName(fbd.SelectedPath) != txtCharID.Text)
                {
                    MessageBox.Show("Invalid Character folder, you should select a folder with a name that corresponds exactly to the character id you put in the appropriate textbox", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            else if (Xenoverse.Xenoverse.characterIds.Contains(txtCharID.Text))
            {
                MessageBox.Show("Names of characters that are already present in the game are not allowed. (Ex. GOK)", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

    }
}