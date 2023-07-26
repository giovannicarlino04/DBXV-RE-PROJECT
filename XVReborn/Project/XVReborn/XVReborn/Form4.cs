using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using XVReborn.Properties;
using FreeImageAPI;
using System.Linq;
using System.Diagnostics;

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

            int count = 0; // Move the count variable outside the inner loop.

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

                        // Add the DraggableButton to the list.
                        buttonCharacters.Add(buttonCharacter);

                        // Increment the count and check if it exceeds 3.
                        count++;
                        if (count >= 3)
                        {
                            // Add an empty control to act as a line breaker.
                            flowLayoutPanelCharacters.Controls.Add(new Control());
                            count = 0;
                        }
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

                            // Add the DraggableButton to the list.
                            buttonCharacters.Add(buttonCharacter);

                            // Increment the count and check if it exceeds 3.
                            count++;
                            if (count >= 3)
                            {
                                // Add an empty control to act as a line breaker.
                                flowLayoutPanelCharacters.Controls.Add(new Control());
                                count = 0;
                            }
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
            flowLayoutPanelCharacters.FlowDirection = FlowDirection.LeftToRight; // Set FlowDirection to LeftToRight.
            flowLayoutPanelCharacters.WrapContents = false; // Prevent wrapping to maintain a single row.
            flowLayoutPanelCharacters.AutoScroll = true;
            // Add the FlowLayoutPanel control to the form's Controls collection.
            this.Controls.Add(flowLayoutPanelCharacters);

            // Call the function to load character images and add them to the FlowLayoutPanel.
            LoadCharacterImages();
            AddCharacterImagesToFlowLayoutPanel();
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

        private void saveButton_Click(object sender, EventArgs e)
        {
            // Reorder the character slots in the FlowLayoutPanel based on the user's arrangement.
            ReorderCharacterSlots();

            // Save the updated character order in the Charalist.as file.
            SaveCharacterOrderToFile();

            // Remove the semicolon before the square bracket
            RemoveSemicolonBeforeSquareBracket();
        }
    }
}