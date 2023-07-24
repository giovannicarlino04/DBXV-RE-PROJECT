using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using XVReborn.Properties;
using CSharpImageLibrary;
using CSharpImageLibrary.Headers;

namespace XVReborn
{
    public partial class Form4 : Form
    {
        // Assuming you have a class-level variable to store the character codes and their corresponding images.
        Dictionary<string, Image> characterImages = new Dictionary<string, Image>();
        string[][][] charaList; // Class-level variable to store the parsed character data.

        public Form4()
        {
            InitializeComponent();
        }

        // Function to load images and associate them with the character codes.
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
                MessageBox.Show("Character data not found in the AS3 file.");
                return;
            }

            // Parse the character data string into the appropriate data structure.
            charaList = ParseCharacterData(characterDataString);

            foreach (var characterData in charaList)
            {
                string characterCode = characterData[0][0]; // Assuming the character code is in the first element.

                // Check if the image is not already loaded in the dictionary.
                if (!characterImages.ContainsKey(characterCode))
                {
                    string imagePath = Path.Combine(imageFolderPath, characterCode + "_000.dds");

                    // Check if the image file exists before loading.
                    if (File.Exists(imagePath))
                    {
                        try
                        {
                            // Use CSharpImageLibrary to load the DDS file.
                            ImageEngineImage ddsImage = new ImageEngineImage(imagePath);

                            // Get the raw image data.
                            byte[] imageData = ddsImage.OriginalData;

                            // Create a MemoryStream from the image data.
                            using (MemoryStream stream = new MemoryStream(imageData))
                            {
                                // Create a Bitmap from the MemoryStream.
                                Bitmap characterBitmap = new Bitmap(stream);

                                characterImages.Add(characterCode, characterBitmap);
                            }
                        }
                        catch (Exception ex)
                        {
                            // Handle the exception or display an error message.
                            MessageBox.Show($"Error loading image for character {characterCode}: {ex.Message}");
                        }
                    }
                    else
                    {
                        // Handle case where image file does not exist.
                        MessageBox.Show($"Image file not found for character {characterCode}");
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

        // Function to add images to the FlowLayoutPanel.
        void AddCharacterImagesToFlowLayoutPanel()
        {
            // Clear the FlowLayoutPanel before adding images to avoid duplicates when reloading.
            flowLayoutPanelCharacters.Controls.Clear();

            foreach (var characterArray in charaList)
            {
                foreach (var characterData in characterArray)
                {
                    // The character code is the first element in the characterData array.
                    string characterCode = characterData[0].ToString();

                    // Check if the character code exists in the dictionary.
                    if (characterImages.ContainsKey(characterCode))
                    {
                        // Create a PictureBox for the character image.
                        PictureBox pictureBoxCharacter = new PictureBox();
                        pictureBoxCharacter.SizeMode = PictureBoxSizeMode.Zoom;
                        pictureBoxCharacter.Image = characterImages[characterCode];

                        // You can set additional properties for the PictureBox if needed.

                        // Add the PictureBox to the FlowLayoutPanel.
                        flowLayoutPanelCharacters.Controls.Add(pictureBoxCharacter);
                    }
                }
            }
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            // Create the FlowLayoutPanel control.
            flowLayoutPanelCharacters = new FlowLayoutPanel();
            flowLayoutPanelCharacters.Dock = DockStyle.Fill; // Adjust this based on your layout requirements.
                                                             // Set other properties as needed.

            // Add the FlowLayoutPanel control to the form's Controls collection.
            this.Controls.Add(flowLayoutPanelCharacters);

            // Call the function to load character images and add them to the FlowLayoutPanel.
            LoadCharacterImages();
            AddCharacterImagesToFlowLayoutPanel();
        }
    }
}
