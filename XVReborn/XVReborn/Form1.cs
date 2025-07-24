using FreeImageAPI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Management;
using System.Reflection;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using Microsoft.Win32;
using XVReborn.Properties;
using XVReborn.Shared;
using XVReborn.Shared.XV;

namespace XVReborn
{
    public partial class Form1 : Form
    {
        #region Fields
        private string language = "";
        private readonly ModInstaller modInstaller;
        private readonly ModUninstaller modUninstaller;
        private readonly ModConverter modConverter;
        private readonly FileManager fileManager;
        
        // New feature managers
        private readonly ModBackupManager backupManager;
        private readonly ModConflictDetector conflictDetector;
        private readonly ModPriorityManager priorityManager;
        private readonly ModSearchFilter searchFilter;
        private readonly ModInfoPanel modInfoPanel;
        private readonly BatchModOperations batchOperations;
        private readonly ModValidator modValidator;
        private readonly ModDependencyManager dependencyManager;
        private readonly ModEnableDisableManager enableDisableManager;
        private readonly ModInstallationHistory installationHistory;
        #endregion

        #region Constructor
        public Form1()
        {
            InitializeComponent();
            InitializeContextMenu();
            
            // Initialize managers
            modInstaller = new ModInstaller();
            modUninstaller = new ModUninstaller();
            modConverter = new ModConverter();
            fileManager = new FileManager();
            
            // Initialize new feature managers
            backupManager = new ModBackupManager();
            conflictDetector = new ModConflictDetector();
            priorityManager = new ModPriorityManager();
            searchFilter = new ModSearchFilter(lvMods);
            modInfoPanel = new ModInfoPanel();
            batchOperations = new BatchModOperations();
            modValidator = new ModValidator();
            dependencyManager = new ModDependencyManager();
            enableDisableManager = new ModEnableDisableManager();
            installationHistory = new ModInstallationHistory();
            
            // Initialize new features after managers are created
            InitializeNewFeatures();
        }
        #endregion

        #region Initialization
        private void InitializeContextMenu()
        {
            var contextMenu = new ContextMenuStrip();
            
            // Basic operations
            var uninstallMenuItem = new ToolStripMenuItem("Uninstall Mod");
            var enableDisableMenuItem = new ToolStripMenuItem("Enable/Disable Mod");
            var infoMenuItem = new ToolStripMenuItem("Mod Information");
            
            contextMenu.Items.AddRange(new ToolStripItem[] 
            {
                uninstallMenuItem,
                new ToolStripSeparator(),
                enableDisableMenuItem,
                infoMenuItem
            });

            lvMods.ContextMenuStrip = contextMenu;

            uninstallMenuItem.Click += (sender, e) =>
            {
                if (lvMods.SelectedItems.Count > 0)
                {
                    uninstallMod(sender, e);
                }
                else
                {
                    MessageBox.Show("No mod selected for uninstallation.", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            enableDisableMenuItem.Click += (sender, e) =>
            {
                ShowModEnableDisable();
            };

            infoMenuItem.Click += (sender, e) =>
            {
                if (lvMods.SelectedItems.Count > 0)
                {
                    modInfoPanel.UpdateModInfo(lvMods.SelectedItems[0]);
                }
            };

            lvMods.MouseUp += (sender, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    var itemUnderCursor = lvMods.GetItemAt(e.X, e.Y);
                    if (itemUnderCursor != null)
                    {
                        lvMods.SelectedItems.Clear();
                        itemUnderCursor.Selected = true; 
                        contextMenu.Show(lvMods, e.Location);
                    }
                }
            };
        }
        #endregion

        #region New Features Initialization
        private void InitializeNewFeatures()
        {
            // Enable drag and drop for mod priority
            priorityManager.EnableDragAndDrop(lvMods);
            
            // Add mod info panel to form
            modInfoPanel.Location = new Point(lvMods.Right + 10, lvMods.Top);
            modInfoPanel.Size = new Size(300, lvMods.Height);
            this.Controls.Add(modInfoPanel);
            
            // Add selection change handler for mod info
            lvMods.SelectedIndexChanged += (sender, e) =>
            {
                if (lvMods.SelectedItems.Count > 0)
                {
                    modInfoPanel.UpdateModInfo(lvMods.SelectedItems[0]);
                }
                else
                {
                    modInfoPanel.ClearInfo();
                }
            };
            
            // Refresh mod status on load
            enableDisableManager.RefreshModStatus(lvMods);
            
            // Add toolbar for new features
            AddFeatureToolbar();
        }

        private void AddFeatureToolbar()
        {
            var toolbar = new ToolStrip();
            
            // Backup manager
            var btnBackup = new ToolStripButton("Backup");
            btnBackup.Click += (sender, e) => ShowBackupManager();
            toolbar.Items.Add(btnBackup);
            
            // Batch operations
            var btnBatch = new ToolStripButton("Batch Ops");
            btnBatch.Click += (sender, e) => ShowBatchOperations();
            toolbar.Items.Add(btnBatch);
            
            // Priority manager
            var btnPriority = new ToolStripButton("Priority");
            btnPriority.Click += (sender, e) => ShowModPriorityManager();
            toolbar.Items.Add(btnPriority);
            
            // Search & filter
            var btnSearch = new ToolStripButton("Search");
            btnSearch.Click += (sender, e) => ShowSearchFilter();
            toolbar.Items.Add(btnSearch);
            
            // Installation history
            var btnHistory = new ToolStripButton("History");
            btnHistory.Click += (sender, e) => ShowInstallationHistory();
            toolbar.Items.Add(btnHistory);
            
            // Enable/disable all
            var btnEnableAll = new ToolStripButton("Enable All");
            btnEnableAll.Click += (sender, e) => enableDisableManager.EnableAllMods(lvMods);
            toolbar.Items.Add(btnEnableAll);
            
            var btnDisableAll = new ToolStripButton("Disable All");
            btnDisableAll.Click += (sender, e) => enableDisableManager.DisableAllMods(lvMods);
            toolbar.Items.Add(btnDisableAll);
            
            toolbar.Location = new Point(0, MainMenuStrip.Height);
            toolbar.Size = new Size(this.Width, 25);
            this.Controls.Add(toolbar);
        }
        #endregion

        #region Form Events
        public void Form1_Load(object sender, EventArgs e)
        {
            InitializeLanguage();
            CheckVCRedist2013();
            InitializeDataFolder();
            ExtractRequiredFiles();
            LoadModList();
            Clean();
        }

        private void CheckVCRedist2013()
        {
            try
            {
                // Check for MSVCP120.dll (Visual C++ 2013 Redistributable)
                bool msvcp120Found = false;
                
                // Check in System32 (x64) and SysWOW64 (x86)
                string[] searchPaths = {
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.System), "MSVCP120.dll"),
                    Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.SystemX86), "MSVCP120.dll")
                };
                
                foreach (string path in searchPaths)
                {
                    if (File.Exists(path))
                    {
                        msvcp120Found = true;
                        break;
                    }
                }
                
                // Alternative check using registry for Visual C++ 2013 Redistributable
                if (!msvcp120Found)
                {
                    string[] registryKeys = {
                        @"SOFTWARE\Microsoft\VisualStudio\12.0\VC\Runtimes\x86",
                        @"SOFTWARE\Microsoft\VisualStudio\12.0\VC\Runtimes\x64",
                        @"SOFTWARE\WOW6432Node\Microsoft\VisualStudio\12.0\VC\Runtimes\x86",
                        @"SOFTWARE\WOW6432Node\Microsoft\VisualStudio\12.0\VC\Runtimes\x64"
                    };
                    
                    foreach (string keyPath in registryKeys)
                    {
                        using (var key = Registry.LocalMachine.OpenSubKey(keyPath))
                        {
                            if (key != null)
                            {
                                var version = key.GetValue("Version");
                                if (version != null)
                                {
                                    msvcp120Found = true;
                                    break;
                                }
                            }
                        }
                    }
                }
                
                // Alternative check using WMI for more comprehensive detection
                if (!msvcp120Found)
                {
                    try
                    {
                        var searcher = new System.Management.ManagementObjectSearcher(
                            "SELECT * FROM Win32_Product WHERE Name LIKE '%Microsoft Visual C++ 2013%'");
                        var collection = searcher.Get();
                        
                        if (collection.Count > 0)
                        {
                            msvcp120Found = true;
                        }
                    }
                    catch
                    {
                        // WMI might not be available, continue with other checks
                    }
                }
                
                if (!msvcp120Found)
                {
                    var result = MessageBox.Show(
                        "MSVCP120.dll (Visual C++ 2013 Redistributable) is not detected on your system.\n\n" +
                        "This is required for the subtools (XVCharaCreator, XVReplacerCreator, XVSkillCreator) to function properly.\n\n" +
                        "Would you like to download and install it now?\n\n" +
                        "Note: You may need to restart the application after installation.",
                        "VCRedist2013 Required",
                        MessageBoxButtons.YesNo,
                        MessageBoxIcon.Warning);
                    
                    if (result == DialogResult.Yes)
                    {
                        try
                        {
                            // Open the Microsoft download page for VCRedist2013
                            string downloadUrl = "https://aka.ms/highdpimfc2013x86enu";
                            System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                            {
                                FileName = downloadUrl,
                                UseShellExecute = true
                            });
                            
                            MessageBox.Show(
                                "Please download and install the Visual C++ 2013 Redistributable from the opened webpage.\n\n" +
                                "After installation, restart this application for the changes to take effect.",
                                "Installation Instructions",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Information);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(
                                $"Failed to open download page: {ex.Message}\n\n" +
                                "Please manually download Visual C++ 2013 Redistributable from:\n" +
                                "https://aka.ms/highdpimfc2013x86enu",
                                "Error",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // If we can't check for VCRedist, show a warning but don't block the application
                MessageBox.Show(
                    $"Unable to verify Visual C++ 2013 Redistributable installation: {ex.Message}\n\n" +
                    "If you experience issues with the subtools, please ensure VCRedist2013 is installed.",
                    "VCRedist Check Failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning);
            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Clean();
        }
        #endregion

        #region Initialization Methods
        private void InitializeLanguage()
        {
            if (Settings.Default.language.Length == 0)
            {
                var frm = new Form2();
                frm.ShowDialog();
                language = Settings.Default.language;
            }
            else
            {
                language = Settings.Default.language;
            }
            }

        private void InitializeDataFolder()
        {
            if (Properties.Settings.Default.datafolder.Length == 0)
            {
                if (Directory.Exists("D:\\SteamLibrary\\steamapps\\common\\DB Xenoverse\\"))
                {
                    string dataPath = "D:\\SteamLibrary\\steamapps\\common\\DB Xenoverse\\data";
                    Directory.CreateDirectory(dataPath);
                    Settings.Default.datafolder = dataPath;
                    Settings.Default.Save();
                }
                else
                {
                    var gameExe = new OpenFileDialog
                    {
                        Filter = "DBXV.exe|DBXV.exe"
                    };

                    if (gameExe.ShowDialog() == DialogResult.OK)
                    {
                        string dataPath = Path.GetDirectoryName(gameExe.FileName) + @"/data";
                        Directory.CreateDirectory(dataPath);
                        Settings.Default.datafolder = dataPath;
                        Settings.Default.Save();
                    }
                    else
                    {
                        Close();
                    }
                }
            }
            else
            {
                if (!Directory.Exists(Properties.Settings.Default.datafolder))
                    MessageBox.Show("Data Folder not Found, Please Clear Installation", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ExtractRequiredFiles()
        {
            ExtractEmbpack();
            ExtractCharacterFiles();
            ExtractSystemFiles();
            ExtractUIFiles();
            ExtractMessageFiles();
        }

        private void ExtractEmbpack()
        {
            if (!File.Exists(Properties.Settings.Default.datafolder + @"\ui\texture\embpack.exe"))
            {
                var assembly = Assembly.GetExecutingAssembly();
                var stream = assembly.GetManifestResourceStream("XVReborn.ZipFile_Blobs.embpack.zip");
                var archive = new ZipArchive(stream);
                archive.ExtractToDirectory(Path.Combine(Settings.Default.datafolder + @"\ui\texture"));
            }
            }

        private void ExtractCharacterFiles()
        {
            if (!Directory.Exists(Properties.Settings.Default.datafolder + @"\ui\texture\CHARA01"))
            {
                if (!Directory.Exists(Properties.Settings.Default.datafolder + @"\ui\texture"))
                {
                    Directory.CreateDirectory(Properties.Settings.Default.datafolder + @"\ui\texture");
                }

                var assembly = Assembly.GetExecutingAssembly();
                var stream = assembly.GetManifestResourceStream("XVReborn.ZipFile_Blobs.CHARA01.zip");
                var archive = new ZipArchive(stream);

                if (!File.Exists(Path.Combine(Settings.Default.datafolder + @"\ui\texture\CHARA01.emb")))
                    archive.ExtractToDirectory(Path.Combine(Settings.Default.datafolder + @"\ui\texture"));

                RunEmbpackCommand("CHARA01.emb");
            }
        }

        private void ExtractSystemFiles()
        {
            if (!Directory.Exists(Properties.Settings.Default.datafolder + @"\system"))
            {
                Directory.CreateDirectory(Properties.Settings.Default.datafolder + @"\system");

                var assembly = Assembly.GetExecutingAssembly();
                var resourceNames = new[]
                {
                    "XVReborn.ZipFile_Blobs.char_model_spec.zip",
                    "XVReborn.ZipFile_Blobs.chara_sound.zip",
                    "XVReborn.ZipFile_Blobs.parameter_spec_char.zip",
                    "XVReborn.ZipFile_Blobs.aura_setting.zip",
                    "XVReborn.ZipFile_Blobs.custom_skill.zip",
                    "XVReborn.ZipFile_Blobs.XMLSerializer.zip",
                    "XVReborn.ZipFile_Blobs.item.zip"
                };

                foreach (var resourceName in resourceNames)
                {
                    var stream = assembly.GetManifestResourceStream(resourceName);
                    var archive = new ZipArchive(stream);
                archive.ExtractToDirectory(Path.Combine(Settings.Default.datafolder + @"\system"));
                }
            }
        }

        private void ExtractUIFiles()
        {
            if (!Directory.Exists(Properties.Settings.Default.datafolder + @"\ui\iggy"))
            {
                Directory.CreateDirectory(Properties.Settings.Default.datafolder + @"\system");

                var assembly = Assembly.GetExecutingAssembly();
                var charaSeleStream = assembly.GetManifestResourceStream("XVReborn.ZipFile_Blobs.CHARASELE.zip");
                var xvpSlotsStream = assembly.GetManifestResourceStream("XVReborn.ZipFile_Blobs.XVP_SLOTS.zip");

                var charaSeleArchive = new ZipArchive(charaSeleStream);
                var xvpSlotsArchive = new ZipArchive(xvpSlotsStream);

                charaSeleArchive.ExtractToDirectory(Path.Combine(Settings.Default.datafolder + @"\ui\iggy"));

                if (!File.Exists(Settings.Default.datafolder + @"\ui\iggy\XVP_SLOTS.xs"))
                    xvpSlotsArchive.ExtractToDirectory(Path.Combine(Settings.Default.datafolder));
            }
            }

        private void ExtractMessageFiles()
        {
            if (!Directory.Exists(Properties.Settings.Default.datafolder + @"\msg"))
            {
                var assembly = Assembly.GetExecutingAssembly();
                var stream = assembly.GetManifestResourceStream("XVReborn.ZipFile_Blobs.msg.zip");
                var archive = new ZipArchive(stream);
                archive.ExtractToDirectory(Path.Combine(Settings.Default.datafolder + @"\msg"));
            }
            }

        private void LoadModList()
        {
            if (Properties.Settings.Default.modlist.Contains("System.Object"))
            {
                Properties.Settings.Default.modlist.Clear();
            }

            if (Properties.Settings.Default.addonmodlist.Contains("System.Object"))
            {
                Properties.Settings.Default.addonmodlist.Clear();
            }

            loadLvItems();
        }
        #endregion

        #region Utility Methods
        private void RunEmbpackCommand(string fileName)
        {
            var process = new Process();
            var startInfo = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardInput = true,
                UseShellExecute = false
            };

            process.StartInfo = startInfo;
            process.Start();

            using (var sw = process.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    sw.WriteLine("cd " + Properties.Settings.Default.datafolder + @"\ui\texture");
                    sw.WriteLine($@"embpack.exe {fileName}");
                }
            }
        }

        private void Clean()
        {
            fileManager.CleanupTempFiles();
            fileManager.RemoveEmptyDirectories(Settings.Default.datafolder);
        }
        #endregion

        #region Mod Management
        private void installmod(object sender, EventArgs e)
        {
            var ofd = new OpenFileDialog
            {
                Title = "Install Mod",
                Filter = "Xenoverse Mod Files (*.xvmod)|*.xvmod",
                Multiselect = true,
                CheckFileExists = true
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in ofd.FileNames)
                {
                    var modPath = file;
                    
                    // Validate mod before installation
                    var validationResult = modValidator.ValidateMod(modPath);
                    if (!modValidator.ShowValidationDialog(validationResult))
                    {
                        continue;
                    }
                    
                    // Check dependencies
                    var dependencies = dependencyManager.GetModDependencies(modPath);
                    if (!dependencyManager.ShowDependencyDialog(dependencies, Path.GetFileNameWithoutExtension(modPath)))
                    {
                        continue;
                    }
                    
                    // Check for conflicts
                    var conflicts = conflictDetector.DetectConflicts(modPath);
                    if (!conflictDetector.ShowConflictDialog(conflicts, Path.GetFileNameWithoutExtension(modPath)))
                    {
                        continue;
                    }
                    
                    // Create backup before installation
                    backupManager.CreateBackup($"before_install_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}");
                    
                    // Install the mod
                    modInstaller.InstallMod(modPath, language, lvMods);
                    
                    // Log installation
                    installationHistory.LogInstallation(
                        Path.GetFileNameWithoutExtension(modPath),
                        validationResult.ModType,
                        validationResult.ModAuthor,
                        validationResult.ModVersion,
                        true
                    );
                }
                
                // Refresh mod status
                enableDisableManager.RefreshModStatus(lvMods);
            }
            Clean();
        }

        private void uninstallMod(object sender, EventArgs e)
        {
            if (lvMods.SelectedItems.Count == 0)
            {
                MessageBox.Show("No mod selected for uninstallation.", "Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var selectedItem = lvMods.SelectedItems[0];
            var modType = selectedItem.SubItems[2].Text;
            var modName = selectedItem.SubItems[0].Text;

            // Create backup before uninstallation
            backupManager.CreateBackup($"before_uninstall_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}");
            
            // Log uninstallation
            installationHistory.LogUninstallation(modName, modType, "", "", true);
            
            modUninstaller.UninstallMod(modType, modName, language, lvMods);
            Clean();
        }
        #endregion

        #region New Features Event Handlers
        private void ShowBackupManager()
        {
            var backupForm = new Form
            {
                Text = "Backup Manager",
                Size = new Size(500, 400),
                StartPosition = FormStartPosition.CenterParent
            };

            var listBox = new ListBox
            {
                Dock = DockStyle.Top,
                Height = 300
            };

            var btnCreateBackup = new Button
            {
                Text = "Create Backup",
                Location = new Point(10, 320),
                Size = new Size(100, 30)
            };
            btnCreateBackup.Click += (sender, e) =>
            {
                backupManager.CreateBackup();
                RefreshBackupList(listBox);
            };

            var btnRestoreBackup = new Button
            {
                Text = "Restore Backup",
                Location = new Point(120, 320),
                Size = new Size(100, 30)
            };
            btnRestoreBackup.Click += (sender, e) =>
            {
                if (listBox.SelectedItem != null)
                {
                    var backupName = listBox.SelectedItem.ToString();
                    var backupPath = Path.Combine(Settings.Default.datafolder, "backups", $"{backupName}.zip");
                    backupManager.RestoreBackup(backupPath);
                }
            };

            var btnCleanOld = new Button
            {
                Text = "Clean Old Backups",
                Location = new Point(230, 320),
                Size = new Size(120, 30)
            };
            btnCleanOld.Click += (sender, e) =>
            {
                backupManager.CleanOldBackups();
                RefreshBackupList(listBox);
            };

            void RefreshBackupList(ListBox lb)
            {
                lb.Items.Clear();
                var backups = backupManager.GetAvailableBackups();
                foreach (var backup in backups)
                {
                    lb.Items.Add(backup);
                }
            }

            backupForm.Controls.AddRange(new Control[] { listBox, btnCreateBackup, btnRestoreBackup, btnCleanOld });
            RefreshBackupList(listBox);
            backupForm.ShowDialog();
        }

        private void ShowBatchOperations()
        {
            var batchForm = new Form
            {
                Text = "Batch Operations",
                Size = new Size(400, 300),
                StartPosition = FormStartPosition.CenterParent
            };

            var btnBatchInstall = new Button
            {
                Text = "Batch Install Mods",
                Location = new Point(20, 20),
                Size = new Size(150, 30)
            };
            btnBatchInstall.Click += async (sender, e) =>
            {
                var modPaths = batchOperations.GetSelectedModFilePaths();
                if (modPaths.Count > 0)
                {
                    await batchOperations.BatchInstallMods(modPaths, language, lvMods);
                }
            };

            var btnBatchUninstall = new Button
            {
                Text = "Batch Uninstall Mods",
                Location = new Point(20, 60),
                Size = new Size(150, 30)
            };
            btnBatchUninstall.Click += async (sender, e) =>
            {
                var selectedItems = batchOperations.GetSelectedListViewItems(lvMods);
                if (selectedItems.Count > 0)
                {
                    await batchOperations.BatchUninstallMods(selectedItems, language, lvMods);
                }
            };

            var btnSelectAll = new Button
            {
                Text = "Select All",
                Location = new Point(20, 100),
                Size = new Size(150, 30)
            };
            btnSelectAll.Click += (sender, e) => batchOperations.SelectAllMods(lvMods);

            var btnDeselectAll = new Button
            {
                Text = "Deselect All",
                Location = new Point(20, 140),
                Size = new Size(150, 30)
            };
            btnDeselectAll.Click += (sender, e) => batchOperations.DeselectAllMods(lvMods);

            var btnInvertSelection = new Button
            {
                Text = "Invert Selection",
                Location = new Point(20, 180),
                Size = new Size(150, 30)
            };
            btnInvertSelection.Click += (sender, e) => batchOperations.InvertSelection(lvMods);

            batchForm.Controls.AddRange(new Control[] { btnBatchInstall, btnBatchUninstall, btnSelectAll, btnDeselectAll, btnInvertSelection });
            batchForm.ShowDialog();
        }

        private void ShowModPriorityManager()
        {
            var priorityForm = new Form
            {
                Text = "Mod Priority Manager",
                Size = new Size(400, 300),
                StartPosition = FormStartPosition.CenterParent
            };

            var btnMoveUp = new Button
            {
                Text = "Move Up",
                Location = new Point(20, 20),
                Size = new Size(100, 30)
            };
            btnMoveUp.Click += (sender, e) =>
            {
                if (lvMods.SelectedItems.Count > 0)
                {
                    priorityManager.MoveModUp(lvMods, lvMods.SelectedIndices[0]);
                }
            };

            var btnMoveDown = new Button
            {
                Text = "Move Down",
                Location = new Point(130, 20),
                Size = new Size(100, 30)
            };
            btnMoveDown.Click += (sender, e) =>
            {
                if (lvMods.SelectedItems.Count > 0)
                {
                    priorityManager.MoveModDown(lvMods, lvMods.SelectedIndices[0]);
                }
            };

            var btnMoveToTop = new Button
            {
                Text = "Move to Top",
                Location = new Point(20, 60),
                Size = new Size(100, 30)
            };
            btnMoveToTop.Click += (sender, e) =>
            {
                if (lvMods.SelectedItems.Count > 0)
                {
                    priorityManager.MoveModToTop(lvMods, lvMods.SelectedIndices[0]);
                }
            };

            var btnMoveToBottom = new Button
            {
                Text = "Move to Bottom",
                Location = new Point(130, 60),
                Size = new Size(100, 30)
            };
            btnMoveToBottom.Click += (sender, e) =>
            {
                if (lvMods.SelectedItems.Count > 0)
                {
                    priorityManager.MoveModToBottom(lvMods, lvMods.SelectedIndices[0]);
                }
            };

            var lblInfo = new Label
            {
                Text = "You can also drag and drop mods in the main list to reorder them.",
                Location = new Point(20, 120),
                Size = new Size(350, 40),
                Font = new Font("Arial", 9)
            };

            priorityForm.Controls.AddRange(new Control[] { btnMoveUp, btnMoveDown, btnMoveToTop, btnMoveToBottom, lblInfo });
            priorityForm.ShowDialog();
        }

        private void ShowModEnableDisable()
        {
            if (lvMods.SelectedItems.Count == 0)
            {
                MessageBox.Show("Please select a mod first.", "No Mod Selected", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            var selectedItem = lvMods.SelectedItems[0];
            var modName = selectedItem.SubItems[0].Text;
            var modType = selectedItem.SubItems[1].Text;

            var isEnabled = enableDisableManager.IsModEnabled(modName, modType);
            var action = isEnabled ? "disable" : "enable";

            var result = MessageBox.Show($"Do you want to {action} the mod '{modName}'?", 
                $"Confirm {action.ToUpper()}", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result == DialogResult.Yes)
            {
                enableDisableManager.ToggleMod(modName, modType, lvMods);
            }
        }

        private void ShowInstallationHistory()
        {
            installationHistory.ShowHistoryDialog();
        }

        private void ShowSearchFilter()
        {
            var searchForm = new Form
            {
                Text = "Search & Filter Mods",
                Size = new Size(400, 200),
                StartPosition = FormStartPosition.CenterParent
            };

            var lblSearch = new Label
            {
                Text = "Search:",
                Location = new Point(20, 20),
                Size = new Size(60, 20)
            };

            var txtSearch = new TextBox
            {
                Location = new Point(90, 20),
                Size = new Size(200, 20)
            };

            var lblCategory = new Label
            {
                Text = "Category:",
                Location = new Point(20, 50),
                Size = new Size(60, 20)
            };

            var cmbCategory = new ComboBox
            {
                Location = new Point(90, 50),
                Size = new Size(200, 20),
                DropDownStyle = ComboBoxStyle.DropDownList
            };

            var btnApply = new Button
            {
                Text = "Apply Filter",
                Location = new Point(90, 80),
                Size = new Size(100, 30)
            };
            btnApply.Click += (sender, e) =>
            {
                searchFilter.ApplyFilter(txtSearch.Text, cmbCategory.SelectedItem?.ToString());
            };

            var btnClear = new Button
            {
                Text = "Clear Filter",
                Location = new Point(200, 80),
                Size = new Size(100, 30)
            };
            btnClear.Click += (sender, e) =>
            {
                searchFilter.ClearFilter();
                txtSearch.Text = "";
                cmbCategory.SelectedIndex = -1;
            };

            // Populate categories
            var categories = searchFilter.GetAvailableCategories();
            cmbCategory.Items.AddRange(categories.ToArray());

            searchForm.Controls.AddRange(new Control[] { lblSearch, txtSearch, lblCategory, cmbCategory, btnApply, btnClear });
            searchForm.ShowDialog();
        }
        #endregion

        #region UI Event Handlers
        private void clearInstallationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to clear installation?", "Warning", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (Directory.Exists(Properties.Settings.Default.datafolder))
                    Directory.Delete(Properties.Settings.Default.datafolder, true);

                if (Directory.Exists("./XVRebornTemp"))
                    Directory.Delete("./XVRebornTemp", true);

                Properties.Settings.Default.Reset();
                MessageBox.Show("Installation cleared, XVReborn will now close", "Success", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                Close();
            }
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void ConvertX2M(object sender, EventArgs e)
        {
            modConverter.ConvertX2MToXVMod();
                    Clean();
                }

        private void convertXV2ModlooseToolStripMenuItem_Click(object sender, EventArgs e)
        {
            modConverter.ConvertLooseFilesToMod();
        }

        #endregion

        #region ListView Management
        private void saveLvItems()
        {
            Properties.Settings.Default.modlist = new StringCollection();
            Properties.Settings.Default.modlist.AddRange(
                (from i in lvMods.Items.Cast<ListViewItem>()
                 select string.Join("|", 
                     from si in i.SubItems.Cast<ListViewItem.ListViewSubItem>()
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

            lvMods.Items.AddRange(
                (from i in Properties.Settings.Default.modlist.Cast<string>()
                 select new ListViewItem(i.Split('|'))).ToArray());

            label1.Text = "Installed Mods: " + lvMods.Items.Count.ToString();
        }
        #endregion
    }
}
