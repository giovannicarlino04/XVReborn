using System;
using System.IO;
using System.Windows.Forms;
using System.Xml;

namespace XVReborn
{
    public class ModInfoPanel : Panel
    {
        private Label lblModName;
        private Label lblModType;
        private Label lblModAuthor;
        private Label lblModVersion;
        private Label lblModDescription;
        private Label lblFileCount;
        private Label lblInstallDate;
        private Label lblModSize;

        public ModInfoPanel()
        {
            InitializeComponents();
        }

        private void InitializeComponents()
        {
            this.BorderStyle = BorderStyle.FixedSingle;
            this.AutoScroll = true;

            // Title
            var lblTitle = new Label
            {
                Text = "Mod Information",
                Font = new System.Drawing.Font("Arial", 12, System.Drawing.FontStyle.Bold),
                Location = new System.Drawing.Point(10, 10),
                Size = new System.Drawing.Size(200, 20)
            };
            this.Controls.Add(lblTitle);

            // Mod Name
            lblModName = CreateInfoLabel("Name:", 10, 40);
            this.Controls.Add(lblModName);

            // Mod Type
            lblModType = CreateInfoLabel("Type:", 10, 70);
            this.Controls.Add(lblModType);

            // Mod Author
            lblModAuthor = CreateInfoLabel("Author:", 10, 100);
            this.Controls.Add(lblModAuthor);

            // Mod Version
            lblModVersion = CreateInfoLabel("Version:", 10, 130);
            this.Controls.Add(lblModVersion);

            // Mod Description
            lblModDescription = CreateInfoLabel("Description:", 10, 160);
            this.Controls.Add(lblModDescription);

            // File Count
            lblFileCount = CreateInfoLabel("Files:", 10, 220);
            this.Controls.Add(lblFileCount);

            // Install Date
            lblInstallDate = CreateInfoLabel("Installed:", 10, 250);
            this.Controls.Add(lblInstallDate);

            // Mod Size
            lblModSize = CreateInfoLabel("Size:", 10, 280);
            this.Controls.Add(lblModSize);
        }

        private Label CreateInfoLabel(string labelText, int x, int y)
        {
            var label = new Label
            {
                Text = labelText,
                Location = new System.Drawing.Point(x, y),
                Size = new System.Drawing.Size(300, 20),
                Font = new System.Drawing.Font("Arial", 9)
            };
            return label;
        }

        public void UpdateModInfo(ListViewItem selectedItem)
        {
            if (selectedItem == null)
            {
                ClearInfo();
                return;
            }

            try
            {
                var modName = selectedItem.SubItems[0].Text;
                var modType = selectedItem.SubItems[1].Text;

                // Update basic info
                lblModName.Text = $"Name: {modName}";
                lblModType.Text = $"Type: {modType}";

                // Try to load detailed info from mod files
                LoadDetailedModInfo(modName, modType);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error updating mod info: {ex.Message}");
                ClearInfo();
            }
        }

        private void LoadDetailedModInfo(string modName, string modType)
        {
            try
            {
                // Try to find mod's xvmod.xml file
                var xmlPath = FindModXmlFile(modName, modType);
                if (File.Exists(xmlPath))
                {
                    LoadInfoFromXml(xmlPath);
                }
                else
                {
                    LoadBasicInfo(modName, modType);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error loading detailed mod info: {ex.Message}");
                LoadBasicInfo(modName, modType);
            }
        }

        private string FindModXmlFile(string modName, string modType)
        {
            // Look in common locations for the mod's XML file
            var searchPaths = new[]
            {
                Path.Combine(Properties.Settings.Default.datafolder, "installed", $"{modType}_{modName}.xml"),
                Path.Combine(Properties.Settings.Default.datafolder, $"{modName}.xml"),
                Path.Combine(Properties.Settings.Default.datafolder, "mods", $"{modName}.xml")
            };

            foreach (var path in searchPaths)
            {
                if (File.Exists(path))
                    return path;
            }

            return null;
        }

        private void LoadInfoFromXml(string xmlPath)
        {
            try
            {
                var doc = new XmlDocument();
                doc.Load(xmlPath);

                var root = doc.DocumentElement;
                if (root == null) return;

                // Extract mod information from XML
                var name = GetXmlValue(root, "name") ?? "Unknown";
                var author = GetXmlValue(root, "author") ?? "Unknown";
                var version = GetXmlValue(root, "version") ?? "Unknown";
                var description = GetXmlValue(root, "description") ?? "No description available";

                lblModName.Text = $"Name: {name}";
                lblModAuthor.Text = $"Author: {author}";
                lblModVersion.Text = $"Version: {version}";
                lblModDescription.Text = $"Description: {description}";

                // Get file info
                var fileInfo = new FileInfo(xmlPath);
                lblInstallDate.Text = $"Installed: {fileInfo.CreationTime:yyyy-MM-dd HH:mm}";
                lblModSize.Text = $"Size: {GetModSize(xmlPath)}";

                // Count files if possible
                var fileCount = CountModFiles(Path.GetFileNameWithoutExtension(xmlPath));
                lblFileCount.Text = $"Files: {fileCount}";
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error parsing XML: {ex.Message}");
                LoadBasicInfo(Path.GetFileNameWithoutExtension(xmlPath), "Unknown");
            }
        }

        private string GetXmlValue(XmlElement root, string elementName)
        {
            var element = root.SelectSingleNode(elementName);
            return element?.InnerText?.Trim();
        }

        private void LoadBasicInfo(string modName, string modType)
        {
            lblModName.Text = $"Name: {modName}";
            lblModType.Text = $"Type: {modType}";
            lblModAuthor.Text = "Author: Unknown";
            lblModVersion.Text = "Version: Unknown";
            lblModDescription.Text = "Description: No description available";
            lblFileCount.Text = "Files: Unknown";
            lblInstallDate.Text = "Installed: Unknown";
            lblModSize.Text = "Size: Unknown";
        }

        private string GetModSize(string xmlPath)
        {
            try
            {
                var fileInfo = new FileInfo(xmlPath);
                var size = fileInfo.Length;
                
                if (size < 1024)
                    return $"{size} B";
                else if (size < 1024 * 1024)
                    return $"{size / 1024.0:F1} KB";
                else
                    return $"{size / (1024.0 * 1024.0):F1} MB";
            }
            catch
            {
                return "Unknown";
            }
        }

        private int CountModFiles(string modName)
        {
            try
            {
                var installedDir = Path.Combine(Properties.Settings.Default.datafolder, "installed");
                var modFile = Path.Combine(installedDir, $"{modName}.txt");
                
                if (File.Exists(modFile))
                {
                    var files = File.ReadAllLines(modFile);
                    return files.Length;
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error counting mod files: {ex.Message}");
            }

            return 0;
        }

        public void ClearInfo()
        {
            lblModName.Text = "Name: No mod selected";
            lblModType.Text = "Type: -";
            lblModAuthor.Text = "Author: -";
            lblModVersion.Text = "Version: -";
            lblModDescription.Text = "Description: -";
            lblFileCount.Text = "Files: -";
            lblInstallDate.Text = "Installed: -";
            lblModSize.Text = "Size: -";
        }
    }
} 