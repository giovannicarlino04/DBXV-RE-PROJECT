using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using XVReborn.Properties;
using FreeImageAPI;

namespace XVReborn
{

    public partial class Form4 : Form
    {
        public class DraggableButton : Button
        {
            public DraggableButton()
            {
                this.AllowDrop = true;
            }
        }

        List<DraggableButton> buttonCharacters = new List<DraggableButton>(); // Replace ButtonCharacters with buttonCharacters.

        // Assuming you have a class-level variable to store the character codes and their corresponding images.
        Dictionary<string, Image> characterImages = new Dictionary<string, Image>();
        string[][][] charaList; // Class-level variable to store the parsed character data.
        Image defaultImage; // Class-level variable to store the default image.

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
        public Form4()
        {
            InitializeComponent();
            LoadDefaultImage();
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

                        // Add the DraggableButton to the list and the FlowLayoutPanel.
                        buttonCharacters.Add(buttonCharacter);
                        flowLayoutPanelCharacters.Controls.Add(buttonCharacter);
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

                            // Wire up the DragEnter, DragDrop, and MouseMove events for the DraggableButton.
                            buttonCharacter.DragEnter += ButtonCharacter_DragEnter;
                            buttonCharacter.DragDrop += ButtonCharacter_DragDrop;
                            buttonCharacter.MouseMove += ButtonCharacter_MouseMove;

                            // Add the DraggableButton to the list and the FlowLayoutPanel.
                            buttonCharacters.Add(buttonCharacter);
                            flowLayoutPanelCharacters.Controls.Add(buttonCharacter);
                        }
                    }
                }
            }
        }
        
        private void flowLayoutPanelCharacters_ControlAdded(object sender, ControlEventArgs e)
        {
            if (flowLayoutPanelCharacters.Controls.Count % 3 == 0)
                flowLayoutPanelCharacters.SetFlowBreak(e.Control as Control, true);
        }

        private void Form4_Load(object sender, EventArgs e)
        {
            // Create the FlowLayoutPanel control.
            flowLayoutPanelCharacters = new FlowLayoutPanel();
            flowLayoutPanelCharacters.Dock = DockStyle.Fill; // Adjust this based on your layout requirements.
            flowLayoutPanelCharacters.ControlAdded += new System.Windows.Forms.ControlEventHandler(flowLayoutPanelCharacters_ControlAdded);
            flowLayoutPanelCharacters.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanelCharacters.HorizontalScroll.Enabled = true;

            // Add the FlowLayoutPanel control to the form's Controls collection.
            this.Controls.Add(flowLayoutPanelCharacters);

            // Call the function to load character images and add them to the FlowLayoutPanel.
            LoadCharacterImages();
            AddCharacterImagesToFlowLayoutPanel();
        }

        // Function to handle the reordering of character slots in the FlowLayoutPanel.
        void ReorderCharacterSlots()
        {
            // Create a list to store the ordered character codes.
            List<string> orderedCharacterCodes = new List<string>();

            // Iterate through the DraggableButton controls in the FlowLayoutPanel.
            foreach (DraggableButton buttonCharacter in flowLayoutPanelCharacters.Controls)
            {
                // Get the character code associated with the DraggableButton.
                string characterCode = buttonCharacter.Tag.ToString();

                // Add the character code to the ordered list.
                orderedCharacterCodes.Add(characterCode);
            }

            // Update the charaList array with the new order.
            for (int i = 0; i < charaList.Length; i++)
            {
                // Find the index of the character code in the ordered list.
                int newIndex = orderedCharacterCodes.IndexOf(charaList[i][0][0]);

                // If the character code is found in the ordered list, update its position in charaList.
                if (newIndex >= 0)
                {
                    string[][] temp = charaList[newIndex];
                    charaList[newIndex] = charaList[i];
                    charaList[i] = temp;
                }
            }
        }
        // Function to save the updated order in the Charalist.as file.
        void SaveCharacterOrderToFile()
        {
            // Replace "CharaListDlc0_0" with the actual array containing the character codes.
            string charaListFilePath = Properties.Settings.Default.flexsdkfolder + @"/bin/scripts/action_script/Charalist.as";

            // Read the existing content of Charalist.as.
            string[] charaListLines = File.ReadAllLines(charaListFilePath);

            // Assuming the character data is stored in CharaListDlc0_0 variable in the AS3 file.
            string charaListVariable = "CharaListDlc0_0";
            string startToken = charaListVariable + ":Array = [";
            string endToken = "]];";
            string characterDataString = "";

            // Find the line that contains the character data and update it.
            for (int i = 0; i < charaListLines.Length; i++)
            {
                if (charaListLines[i].Contains(startToken))
                {
                    // Generate the new character data string based on the updated charaList.
                    characterDataString = GenerateCharacterDataString(charaListLines[i], charaList);

                    // Update the line with the new character data.
                    charaListLines[i] = charaListVariable + ":Array = [" + characterDataString + "]];";
                    break;
                }
            }

            // Write the updated content back to Charalist.as.
            File.WriteAllLines(charaListFilePath, charaListLines);

            // Display a message indicating successful save.
            MessageBox.Show("Character order saved successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        // Function to generate the character data string based on the updated charaList.
        private string GenerateCharacterDataString(string startTokenLine, string[][][] updatedCharaList)
        {
            // Get the start index of the character data string.
            int startIndex = startTokenLine.IndexOf(":Array = [") + ":Array = [".Length;

            // Get the end index of the character data string.
            int endIndex = startTokenLine.LastIndexOf("]];", StringComparison.Ordinal) + 3;

            // Extract the original character data string from the startTokenLine.
            string originalCharacterDataString = startTokenLine.Substring(startIndex, endIndex - startIndex);

            // Create a new character data string based on the updated charaList.
            string newCharacterDataString = "";
            for (int i = 0; i < charaList.Length; i++)
            {
                newCharacterDataString += "[";
                for (int j = 0; j < charaList[i].Length; j++)
                {
                    string characterCode = charaList[i][j][0];
                    string costumeID1 = charaList[i][j][1];
                    string costumeID2 = charaList[i][j][2];
                    string costumeID3 = charaList[i][j][3];
                    string voiceID1 = charaList[i][j][4];
                    string voiceID2 = charaList[i][j][5];

                    // Create a character data string for the current button.
                    string characterDataString = "[[\"" + characterCode + "\"," + costumeID1 + "," + costumeID2 + "," + costumeID3 + ",[" + voiceID1 + "," + voiceID2 + "]]]";
                    newCharacterDataString += characterDataString;

                    if (j < charaList[i].Length - 1)
                    {
                        newCharacterDataString += ",";
                    }
                }
                newCharacterDataString += "]";
                if (i < charaList.Length - 1)
                {
                    newCharacterDataString += ",";
                }
            }

            // Return the updated character data string.
            return newCharacterDataString;
        }
        private void saveButton_Click(object sender, EventArgs e)
        {
            // Reorder the character slots in the FlowLayoutPanel based on the user's arrangement.
            ReorderCharacterSlots();

            // Save the updated character order in the Charalist.as file.
            SaveCharacterOrderToFile();
        }
    }
}