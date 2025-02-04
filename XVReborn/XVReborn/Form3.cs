using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using FreeImageAPI;
using XVReborn.Properties;

namespace XVReborn
{
    public partial class Form3 : Form
    {
        public class DraggableButton : Button
        {
            public DraggableButton()
            {
                this.AllowDrop = true;
            }
        }
        private List<string[]> characterOrder = new List<string[]>();

        private FlowLayoutPanel flowLayoutPanelCharacters;
        private List<DraggableButton> buttonCharacters = new List<DraggableButton>();
        private Dictionary<string, Image> characterImages = new Dictionary<string, Image>();
        private Image defaultImage;

        public Form3()
        {
            InitializeComponent();
            LoadDefaultImage();
        }

        private void LoadDefaultImage()
        {
            string defaultImagePath = Path.Combine(Settings.Default.datafolder, @"ui\texture\CHARA01", "FOF_000.dds");

            if (File.Exists(defaultImagePath))
            {
                try
                {
                    FREE_IMAGE_FORMAT imageFormat = FREE_IMAGE_FORMAT.FIF_DDS;
                    FIBITMAP dib = FreeImage.LoadEx(defaultImagePath, ref imageFormat);
                    if (dib != null)
                    {
                        Bitmap defaultBitmap = FreeImage.GetBitmap(dib);
                        FreeImage.UnloadEx(ref dib);

                        defaultImage?.Dispose();
                        defaultImage = defaultBitmap;
                    }
                    else
                    {
                        Console.WriteLine("Error: Image data is null");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Error loading default image: " + ex.Message);
                }
            }
            else
            {
                Console.WriteLine("Default image file not found.");
            }
        }
        private void ReorderCharacterSlots()
        {
            var orderedCharacterData = new List<string[]>();

            foreach (DraggableButton buttonCharacter in flowLayoutPanelCharacters.Controls)
            {
                string characterCode = buttonCharacter.Tag.ToString();

                var characterData = characterOrder.FirstOrDefault(c => c[0] == characterCode);
                if (characterData != null)
                {
                    orderedCharacterData.Add(characterData);
                }
            }

            string charaListFilePath = Path.Combine(Settings.Default.datafolder, "XVP_SLOTS.xs");

            if (File.Exists(charaListFilePath))
            {
                characterOrder = orderedCharacterData;  // Update the list with the new order

                // Flatten the characterOrder and write it to the file in one line
                string singleLineData = string.Join("}{", characterOrder
                    .Select(characterData =>  string.Join(",", characterData.Select(data => data.Trim())) ));

                // Save the data as a single line
                File.WriteAllText(charaListFilePath, singleLineData);
            }
            else
            {
                Console.WriteLine("Character file not found.");
            }
        }


        private void ButtonCharacter_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                DraggableButton buttonCharacter = (DraggableButton)sender;  // Ensure we're getting the correct button
                buttonCharacter.DoDragDrop(buttonCharacter, DragDropEffects.Move);  // Start the drag and drop operation
            }
        }

        private void ButtonCharacter_DragEnter(object sender, DragEventArgs e)
        {
            e.Effect = DragDropEffects.Move;
        }

        private void ButtonCharacter_DragDrop(object sender, DragEventArgs e)
        {
            var targetButton = (DraggableButton)sender;
            var sourceButton = (DraggableButton)e.Data.GetData(typeof(DraggableButton));

            // Only move and update the order if the source and target buttons are different
            int targetIndex = flowLayoutPanelCharacters.Controls.GetChildIndex(targetButton);
            int sourceIndex = flowLayoutPanelCharacters.Controls.GetChildIndex(sourceButton);

            if (sourceIndex != targetIndex)
            {
                flowLayoutPanelCharacters.Controls.SetChildIndex(sourceButton, targetIndex);
                buttonCharacters.RemoveAt(sourceIndex);
                buttonCharacters.Insert(targetIndex, sourceButton);

                ReorderCharacterSlots();
            }

            // Ensure the image is not reloaded unnecessarily after drag-and-drop
            if (targetButton.BackgroundImage == null && characterImages.ContainsKey(targetButton.Tag.ToString()))
            {
                targetButton.BackgroundImage = characterImages[targetButton.Tag.ToString()];
            }
        }
        private void LoadCharacterImage(string characterCode, string imageFolderPath)
        {
            if (characterImages.ContainsKey(characterCode)) return; // Don't load if the image already exists

            try
            {
                Regex regex = new Regex($@"^{Regex.Escape(characterCode)}_.+\.dds$", RegexOptions.IgnoreCase);
                string[] matchingFiles = Directory.GetFiles(imageFolderPath)
                    .Where(f => regex.IsMatch(Path.GetFileName(f)))
                    .ToArray();

                string selectedImagePath = matchingFiles.Length > 0 ? matchingFiles[0] : "FOF_000.dds";

                Console.WriteLine("Found: " + selectedImagePath);

                FREE_IMAGE_FORMAT imageFormat = FREE_IMAGE_FORMAT.FIF_DDS;
                FIBITMAP dib = FreeImage.LoadEx(selectedImagePath, ref imageFormat);
                if (dib != null)
                {
                    Bitmap characterBitmap = FreeImage.GetBitmap(dib);
                    FreeImage.UnloadEx(ref dib);

                    characterImages[characterCode] = characterBitmap;
                }
                else
                {
                    characterImages[characterCode] = defaultImage ?? new Bitmap(defaultImage);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error loading image for {characterCode}: {ex.Message}");
                characterImages[characterCode] = defaultImage ?? new Bitmap(defaultImage);
            }
        }
        private List<string[]> ParseCharacterData(string characterDataString)
        {
            characterDataString = characterDataString.Substring(2, characterDataString.Length - 4);

            if (string.IsNullOrEmpty(characterDataString)) return new List<string[]>();

            string[] characterDataBlocks = characterDataString.Split(new string[] { "}{" }, StringSplitOptions.None);
            var characterList = new List<string[]>();

            for (int i = 0; i < characterDataBlocks.Length; i++)
            {
                string cleanedData = characterDataBlocks[i].Trim(new char[] { '[', ']' });
                string[] characterEntries = cleanedData.Split(new string[] { "][" }, StringSplitOptions.None);

                foreach (string entry in characterEntries)
                {
                    string[] attributes = entry.Split(',');

                    if (attributes.Length != 6)
                    {
                        Console.WriteLine($"Warning: Incomplete character data at index {i}, skipping.");
                        continue;
                    }

                    // Debug output to ensure data is correct
                    Console.WriteLine($"Parsed character data: {string.Join(",", attributes)}");

                    // Add the parsed attributes to the list
                    characterList.Add(attributes.Select(attr => attr.Trim()).ToArray());
                }
            }

            return characterList;
        }

        private void AddCharacterImagesToFlowLayoutPanel()
        {
            flowLayoutPanelCharacters.Controls.Clear();

            foreach (var characterData in characterOrder) // Use characterOrder instead of charaList
            {
                string characterCode = characterData[0];

                var buttonCharacter = new DraggableButton
                {
                    BackgroundImageLayout = ImageLayout.Zoom,
                    Width = 128,
                    Height = 64,
                    Tag = characterCode
                };

                // Check if the image is already loaded, otherwise use the default image
                buttonCharacter.BackgroundImage = characterImages.ContainsKey(characterCode)
                    ? characterImages[characterCode]
                    : defaultImage ?? new Bitmap(defaultImage);

                // Attach drag and drop event handlers
                buttonCharacter.DragEnter += ButtonCharacter_DragEnter;
                buttonCharacter.DragDrop += ButtonCharacter_DragDrop;
                buttonCharacter.MouseMove += ButtonCharacter_MouseMove;

                // Add button to the FlowLayoutPanel
                flowLayoutPanelCharacters.Controls.Add(buttonCharacter);
            }
        }

        private void LoadCharacterImages()
        {
            string imageFolderPath = Path.Combine(Settings.Default.datafolder, @"ui\texture\CHARA01");
            string[] characterDataString = File.ReadAllLines(Path.Combine(Settings.Default.datafolder, "XVP_SLOTS.xs"));

            // Ensure the file isn't empty before processing
            if (characterDataString.Length == 0 || string.IsNullOrEmpty(characterDataString[0])) return;

            // Populate characterOrder with the parsed character data
            characterOrder = ParseCharacterData(string.Join("", characterDataString)); // Join lines if needed

            foreach (var characterData in characterOrder)
            {
                string characterCode = characterData[0];

                var buttonCharacter = new DraggableButton
                {
                    BackgroundImageLayout = ImageLayout.Zoom,
                    Width = 128,
                    Height = 64,
                    Tag = characterCode
                };

                // Check if the image already exists in the dictionary
                if (characterImages.ContainsKey(characterCode))
                {
                    buttonCharacter.BackgroundImage = characterImages[characterCode];
                }
                else
                {
                    buttonCharacter.BackgroundImage = defaultImage ?? new Bitmap(defaultImage);
                }

                // Attach drag and drop event handlers
                buttonCharacter.DragEnter += ButtonCharacter_DragEnter;
                buttonCharacter.DragDrop += ButtonCharacter_DragDrop;
                buttonCharacter.MouseMove += ButtonCharacter_MouseMove;

                // Add button to list and panel
                buttonCharacters.Add(buttonCharacter);
                flowLayoutPanelCharacters.Controls.Add(buttonCharacter);

                // Load character images if not already loaded
                if (!characterImages.ContainsKey(characterCode))
                {
                    LoadCharacterImage(characterCode, imageFolderPath);
                }
            }
        }

        private void flowLayoutPanelCharacters_ControlAdded(object sender, ControlEventArgs e)
        {
            if (e.Control is DraggableButton buttonCharacter)
            {
                int index = flowLayoutPanelCharacters.Controls.IndexOf(buttonCharacter);
                int columnIndex = index % 3;

                flowLayoutPanelCharacters.SetFlowBreak(buttonCharacter, columnIndex == 2 && index < flowLayoutPanelCharacters.Controls.Count - 1);
            }
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            flowLayoutPanelCharacters = new FlowLayoutPanel
            {
                Dock = DockStyle.Fill,
                FlowDirection = FlowDirection.TopDown,
                AutoScroll = true
            };
            flowLayoutPanelCharacters.ControlAdded += flowLayoutPanelCharacters_ControlAdded;
            this.Controls.Add(flowLayoutPanelCharacters);

            LoadCharacterImages();
            AddCharacterImagesToFlowLayoutPanel();
        }

        private void Form3_FormClosing(object sender, FormClosingEventArgs e)
        {
        }
    }
}
