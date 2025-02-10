using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;

namespace XVSkillCreator
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            InitializeCUS();
        }
        private Dictionary<string, TextBox> textBoxDictionary = new Dictionary<string, TextBox>();

        private void InitializeCUS()
        {
            // Create the "CUS" TabPage
            TabPage cusTabPage = new TabPage("CUS");
            tabControl1.TabPages.Add(cusTabPage);

            // Create a Panel to contain all the controls (labels + textboxes)
            Panel panel = new Panel();
            panel.Location = new Point(10, 10);
            panel.Size = new Size(800, 400); // Set the panel size (adjust as necessary)
            panel.AutoScroll = true; // Enable automatic scrolling for the panel
            cusTabPage.Controls.Add(panel);

            // Create TextBoxes dynamically inside the "CUS" TabPage
            string[] fields = { "ShortName", "ID1", "ID2", "I_04", "Race_Lock", "Type", "FilesLoaded",
                        "PartSet", "I_18", "EAN", "CAM_EAN", "EEPK", "ACB_SE", "ACB_VOX",
                        "AFTER_BAC", "AFTER_BCM", "I_48", "I_50", "I_52", "I_54", "PUP",
                        "CUS_Aura", "TransformCharaSwap", "Skillset_Change", "Num_Of_Transforms", "I_66" };

            int yOffset = 20;  // Starting Y position for the TextBoxes
            int textBoxHeight = 20; // Height of each TextBox (smaller height)
            int labelWidth = 90; // Width of the label (adjusted)
            int textBoxWidth = 120; // Width of the textboxes (smaller)

            foreach (var field in fields)
            {
                // Create Label
                Label label = new Label();
                label.Text = field;
                label.Location = new Point(10, yOffset);
                label.Width = labelWidth;

                // Create TextBox
                TextBox textBox = new TextBox();
                textBox.Name = field; // Name each textbox by its field name
                textBox.Location = new Point(label.Width + 15, yOffset); // Adjusted for proper spacing
                textBox.Width = textBoxWidth; // Set width of the textboxes smaller
                textBox.Height = textBoxHeight; // Set height of the textboxes smaller

                // Add Label and TextBox to the Panel
                panel.Controls.Add(label);
                panel.Controls.Add(textBox);
                textBoxDictionary[field] = textBox;

                // Update the Y offset for the next control (next line)
                yOffset += textBoxHeight + 10;
            }
            VScrollBar vScrollBar = new VScrollBar();
            vScrollBar.Dock = DockStyle.Right; // Dock the scrollbar to the right side of the panel
            vScrollBar.Minimum = 0;
            vScrollBar.Maximum = yOffset; // Set the maximum value of the scrollbar based on the number of controls
            panel.Controls.Add(vScrollBar);
            vScrollBar.ValueChanged += (sender, e) =>
            {
                panel.AutoScrollPosition = new Point(0, vScrollBar.Value); // Scroll the panel based on scrollbar value
            };
        }




        private void buildXVMODFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                Title = "Save Mod File",
                Filter = "Xenoverse Mod Files (*.xvmod)|*.xvmod"
            };

            if (txtModName.Text.Length > 0 && txtModAuthor.Text.Length > 0
                && Directory.Exists(txtSkillFiles.Text)
                && sfd.ShowDialog() == DialogResult.OK)
            {
                string tempDir = "./XVSkillCreatorTemp";
                string xmlFilePath = Path.Combine(tempDir, "xvmod.xml");

                if (Directory.Exists(tempDir))
                    Directory.Delete(tempDir, true);
                Directory.CreateDirectory(tempDir);
                Directory.CreateDirectory(Path.Combine(tempDir, "skill"));

                string skillType = cbSkillType.SelectedItem.ToString() switch
                {
                    "Ultimate" => "ULT",
                    "Evasive" => "ESC",
                    _ => "SPA"
                };
                Directory.CreateDirectory(Path.Combine(tempDir, "skill", skillType));

                XmlWriterSettings settings = new XmlWriterSettings { Indent = true, IndentChars = "    " };

                using (XmlWriter writer = XmlWriter.Create(xmlFilePath, settings))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("XVMOD");
                    writer.WriteAttributeString("type", "ADDED_SKILL");

                    WriteElementWithValue(writer, "MOD_NAME", txtModName.Text);
                    WriteElementWithValue(writer, "MOD_AUTHOR", txtModAuthor.Text);
                    WriteElementWithValue(writer, "SKILL_TYPE", skillType);
                    WriteElementWithValue(writer, "MSG_SKILL_NAME", tbSkName.Text);
                    WriteElementWithValue(writer, "MSG_SKILL_DESC", tbSkDesc.Text);

                    string[] fields = { "ShortName", "ID1", "ID2", "I_04", "Race_Lock", "Type", "FilesLoaded",
                        "PartSet", "I_18", "EAN", "CAM_EAN", "EEPK", "ACB_SE", "ACB_VOX",
                        "AFTER_BAC", "AFTER_BCM", "I_48", "I_50", "I_52", "I_54", "PUP",
                        "CUS_Aura", "TransformCharaSwap", "Skillset_Change", "Num_Of_Transforms", "I_66" };

                    foreach (var field in fields)
                    {
                        var control = textBoxDictionary[field];
                        if (control != null)
                        {
                            WriteElementWithValue(writer, field, control.Text);
                        }
                        else
                        {
                            WriteElementWithValue(writer, field, "0");
                        }
                    }

                    writer.WriteEndElement();
                    writer.WriteEndDocument();
                }
                string skillFolderPath = txtSkillFiles.Text;
                CopyDirectory(skillFolderPath, tempDir + $"/skill/{skillType}");

                Directory.CreateDirectory(Path.Combine(tempDir, "JUNGLE"));

                if (txtAdditionalFiles.Text.Length > 0)
                {
                    string sourceAdditionalFilesDir = txtAdditionalFiles.Text;
                    string destinationAdditionalFilesDir = Path.Combine(tempDir, "JUNGLE");

                    Console.WriteLine($"Source Additional Files Directory: {sourceAdditionalFilesDir}");
                    Console.WriteLine($"Destination Additional Files Directory: {destinationAdditionalFilesDir}");

                    if (Directory.Exists(sourceAdditionalFilesDir))
                    {
                        if (Directory.GetFiles(sourceAdditionalFilesDir).Length > 0 || Directory.GetDirectories(sourceAdditionalFilesDir).Length > 0)
                        {
                            CopyDirectory(sourceAdditionalFilesDir, destinationAdditionalFilesDir);
                        }
                        else
                        {
                            Console.WriteLine("The source directory is empty. No files to copy.");
                        }
                    }
                    else
                    {
                        MessageBox.Show($"The source directory {sourceAdditionalFilesDir} does not exist.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                ZipFile.CreateFromDirectory(tempDir, sfd.FileName);

                if (File.Exists(xmlFilePath))
                    File.Delete(xmlFilePath);

                MessageBox.Show("Mod Created Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Directory.Delete(tempDir, true);
            }
        }
        public static void CopyDirectory(string sourceDir, string destDir, bool recursive = true)
        {
            if (string.IsNullOrEmpty(sourceDir))
                throw new ArgumentNullException(nameof(sourceDir), "Source directory cannot be null or empty.");

            if (!Directory.Exists(sourceDir))
                throw new DirectoryNotFoundException($"Source directory not found: {sourceDir}");

            if (Directory.Exists(destDir))
                Directory.Delete(destDir, recursive);  

            Directory.CreateDirectory(destDir);

            var files = Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories);
            if (files.Length == 0)
                throw new FileNotFoundException($"No files found in directory: {sourceDir}");

            foreach (string file in files)
            {
                string destFile = file.Replace(sourceDir, destDir);
                Directory.CreateDirectory(Path.GetDirectoryName(destFile)); 
                File.Copy(file, destFile, true); 
            }
        }


        private void WriteElementWithValue(XmlWriter writer, string elementName, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (value.StartsWith("\"") && value.EndsWith("\""))
                    value = value[1..^1];

                writer.WriteStartElement(elementName);
                writer.WriteAttributeString("value", value);
                writer.WriteEndElement();
            }
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                Title = "Open Mod File",
                Filter = "Xenoverse Mod Files (*.xvmod)|*.xvmod"
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string tempDir = "./XVSkillCreatorTemp2";

                Console.WriteLine($"Opening file: {ofd.FileName}");

                if (Directory.Exists(tempDir))
                {
                    Console.WriteLine($"Deleting existing temp directory: {tempDir}");
                    Directory.Delete(tempDir, true);
                }
                Directory.CreateDirectory(tempDir);
                ZipFile.ExtractToDirectory(ofd.FileName, tempDir);

                string xmlFilePath = Path.Combine(tempDir, "xvmod.xml");
                if (!File.Exists(xmlFilePath))
                {
                    MessageBox.Show("Error: xvmod.xml not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                Console.WriteLine($"Parsing XML file: {xmlFilePath}");
                XDocument xmlDoc = XDocument.Load(xmlFilePath);

                // Extract values safely using helper function
                txtModName.Text = GetElementValue(xmlDoc, "MOD_NAME");
                txtModAuthor.Text = GetElementValue(xmlDoc, "MOD_AUTHOR");
                cbSkillType.SelectedItem = GetElementValue(xmlDoc, "SKILL_TYPE");
                tbSkName.Text = GetElementValue(xmlDoc, "MSG_SKILL_NAME");
                tbSkDesc.Text = GetElementValue(xmlDoc, "MSG_SKILL_DESC");

                string modType = xmlDoc.Descendants("XVMOD").FirstOrDefault()?.Attribute("type")?.Value?.Trim() ?? "";
                Console.WriteLine($"Mod Type: {modType}");

                if (modType != "ADDED_SKILL")
                {
                    throw new NotImplementedException($"Mod type {modType} not supported");
                }

                string skillType = cbSkillType.SelectedItem != null
                    ? cbSkillType.SelectedItem.ToString() switch
                    {
                        "Ultimate" => "ULT",
                        "Evasive" => "ESC",
                        _ => "SPA"
                    }
                    : "SPA"; // Default value if nothing is selected

                Console.WriteLine($"Resolved Skill Type: {skillType}");

                // Locate the skill folder
                string skillFolderBase = Path.Combine(tempDir, "skill", skillType);
                Console.WriteLine($"Looking for skill files in: {skillFolderBase}");

                if (Directory.Exists(skillFolderBase))
                {
                    string firstSkillSubFolder = Directory.GetDirectories(skillFolderBase).FirstOrDefault() ?? "";
                    txtSkillFiles.Text = firstSkillSubFolder;
                    Console.WriteLine($"Set txtSkillFiles.Text to: {firstSkillSubFolder}");
                }
                else
                {
                    Console.WriteLine("No skill subdirectories found!");
                    txtSkillFiles.Text = "";
                }

                // Load additional fields safely
                foreach (var field in textBoxDictionary.Keys)
                {
                    string value = GetElementValue(xmlDoc, field);
                    textBoxDictionary[field].Text = value;
                    Console.WriteLine($"Field: {field}, Value: {value}");
                }

                string junglePath = Path.Combine(tempDir, "JUNGLE");
                if (Directory.Exists(junglePath))
                {
                    txtAdditionalFiles.Text = junglePath;
                    Console.WriteLine($"Additional files path: {junglePath}");
                }
                else
                {
                    txtAdditionalFiles.Text = "";
                    Console.WriteLine("No additional files found.");
                }
            }
        }
        // Helper method to safely get XML values from the "value" attribute
        private string GetElementValue(XDocument xmlDoc, string elementName)
        {
            return xmlDoc.Descendants(elementName)
                         .FirstOrDefault()?
                         .Attribute("value")?
                         .Value
                         .Trim() ?? "0";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            cbSkillType.SelectedIndex = 0;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            using FolderBrowserDialog fbd = new FolderBrowserDialog { Description = "Select skill folder" };
            if (fbd.ShowDialog() == DialogResult.OK)
                txtSkillFiles.Text = fbd.SelectedPath;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            using FolderBrowserDialog fbd = new FolderBrowserDialog { Description = "Select Additional data folder" };
            if (fbd.ShowDialog() == DialogResult.OK)
                txtAdditionalFiles.Text = fbd.SelectedPath;
        }
        public static string GetDynamicDataFolder()
        {
            string appDataFolder = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string baseXVRebornPath = Path.Combine(appDataFolder, "XVReborn");

            if (Directory.Exists(baseXVRebornPath))
            {
                var dynamicFolder = Directory.GetDirectories(baseXVRebornPath, "XVReborn.exe_Url_*")
                                              .FirstOrDefault(); // Use FirstOrDefault to get the first match

                if (dynamicFolder != null)
                {
                    string versionFolder = Path.Combine(dynamicFolder, "1.0.0.0");
                    if (Directory.Exists(versionFolder))
                    {
                        try
                        {
                            string configFilePath = Path.Combine(versionFolder, "user.config");  // Name the XML file as per your project
                            if (File.Exists(configFilePath))
                            {
                                XDocument xmlDoc = XDocument.Load(configFilePath);
                                var dataFolderElement = xmlDoc.Descendants("setting")
                                                              .FirstOrDefault(s => (string)s.Attribute("name") == "datafolder");
                                if (dataFolderElement != null)
                                {
                                    return dataFolderElement.Element("value")?.Value ?? string.Empty;
                                }
                                else
                                {
                                    throw new InvalidOperationException("datafolder setting not found.");
                                }
                            }
                            else
                            {
                                throw new FileNotFoundException($"Config file not found at {configFilePath}");
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error: {ex.Message}");
                            return string.Empty;
                        }
                    }
                    else
                    {
                        MessageBox.Show($"Version folder not found: {versionFolder}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return string.Empty; // Or handle accordingly
                    }
                }
                else
                {
                    MessageBox.Show("Dynamic XVReborn folder not found.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return string.Empty; // Or handle accordingly
                }
            }
            else
            {
                MessageBox.Show("XVReborn folder not found. Start XVReborn first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return string.Empty; // Or handle accordingly
            }
        }
        public static string ShowSkillSelection(List<string> allSkills)
        {
            var skillSelectionForm = new Form
            {
                Text = "Select a Skill",
                Size = new System.Drawing.Size(300, 400)
            };

            ListBox skillListBox = new ListBox
            {
                DataSource = allSkills,
                Dock = DockStyle.Top,
                Height = 300
            };

            Button selectButton = new Button
            {
                Text = "Select",
                Dock = DockStyle.Bottom
            };
            selectButton.Click += (sender, e) =>
            {
                if (skillListBox.SelectedItem != null)
                {
                    string selectedSkill = skillListBox.SelectedItem.ToString();
                    skillSelectionForm.Tag = selectedSkill; // Store the selected skill
                    skillSelectionForm.DialogResult = DialogResult.OK;
                    skillSelectionForm.Close();
                }
                else
                {
                    MessageBox.Show("Please select a skill.", "No Skill Selected", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            };

            Button cancelButton = new Button
            {
                Text = "Cancel",
                Dock = DockStyle.Bottom
            };
            cancelButton.Click += (sender, e) =>
            {
                skillSelectionForm.DialogResult = DialogResult.Cancel;
                skillSelectionForm.Close();
            };

            skillSelectionForm.Controls.Add(skillListBox);
            skillSelectionForm.Controls.Add(selectButton);
            skillSelectionForm.Controls.Add(cancelButton);

            skillSelectionForm.ShowDialog();

            // Return the selected skill or null if canceled
            return skillSelectionForm.DialogResult == DialogResult.OK ? skillSelectionForm.Tag?.ToString() : null;
        }
        private void copyValuesFromGameToolStripMenuItem_Click(object sender, EventArgs e)
        {
            string datafolder = GetDynamicDataFolder();
            string xvXmlSerializerPath = Path.Combine(datafolder, "system", "XMLSerializer.exe");
            string cusFilePath = Path.Combine(datafolder, "system", "custom_skill.cus");

            if (!File.Exists(xvXmlSerializerPath))
            {
                MessageBox.Show("XMLSerializer.exe not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Console.WriteLine($"Executing: {xvXmlSerializerPath} {cusFilePath}");

            ProcessStartInfo info = new ProcessStartInfo
            {
                FileName = xvXmlSerializerPath,
                Arguments = $"\"{cusFilePath}\"",
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = false
            };

            using (Process p = Process.Start(info))
            {
                p.WaitForExit();
            }

            string cusXmlPath = Path.Combine(datafolder, "system", "custom_skill.cus.xml");

            if (!File.Exists(cusXmlPath))
            {
                MessageBox.Show("Serialized XML file not found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Console.WriteLine("Loading skill data from XML...");

            XDocument cusXmlDoc = XDocument.Load(cusXmlPath);

            // Find all Super, Ultimate, and Evasive skills in the XML
            var superSkills = cusXmlDoc.Descendants("SuperSkills").Elements("Skill").ToList();
            var ultimateSkills = cusXmlDoc.Descendants("UltimateSkills").Elements("Skill").ToList();
            var evasiveSkills = cusXmlDoc.Descendants("EvasiveSkills").Elements("Skill").ToList();

            // Create a list of all skills (Super, Ultimate, and Evasive)
            List<string> allSkills = new List<string>();
            allSkills.AddRange(superSkills.Select(skill => skill.Attribute("ShortName")?.Value ?? "Unknown"));
            allSkills.AddRange(ultimateSkills.Select(skill => skill.Attribute("ShortName")?.Value ?? "Unknown"));
            allSkills.AddRange(evasiveSkills.Select(skill => skill.Attribute("ShortName")?.Value ?? "Unknown"));

            Console.WriteLine($"Found {allSkills.Count} skills.");

            // Show the skills in a selection menu and get the selected skill
            string selectedSkillName = ShowSkillSelection(allSkills);

            if (string.IsNullOrEmpty(selectedSkillName))
            {
                MessageBox.Show("No skill selected!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            Console.WriteLine($"Selected Skill: {selectedSkillName}");

            XElement selectedSkill = superSkills
                .FirstOrDefault(skill => skill.Attribute("ShortName")?.Value == selectedSkillName)
                ?? ultimateSkills.FirstOrDefault(skill => skill.Attribute("ShortName")?.Value == selectedSkillName)
                ?? evasiveSkills.FirstOrDefault(skill => skill.Attribute("ShortName")?.Value == selectedSkillName);

            if (selectedSkill != null)
            {
                Console.WriteLine("Skill found. Populating fields...");

                string[] attributes = { "ShortName", "ID1", "ID2" };
                foreach (string attr in attributes)
                {
                    string value = selectedSkill.Attribute(attr)?.Value ?? "0";
                    if (textBoxDictionary.ContainsKey(attr))
                    {
                        textBoxDictionary[attr].Text = value;
                        Console.WriteLine($"Attribute - {attr}: {value}");
                    }
                }

                foreach (var key in textBoxDictionary.Keys)
                {
                    if (!attributes.Contains(key))
                    {
                        var element = selectedSkill.Element(key);
                        string value = element?.Attribute("value")?.Value ?? "0";

                        textBoxDictionary[key].Text = value;
                        Console.WriteLine($"Element - {key}: {value}");
                    }
                }

                MessageBox.Show("Skill data copied successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }


            else
            {
                MessageBox.Show("Skill not found in the XML!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }




    }
}
