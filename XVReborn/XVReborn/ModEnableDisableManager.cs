using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using XVReborn.Properties;

namespace XVReborn
{
    public class ModEnableDisableManager
    {
        private readonly string dataFolder;
        private readonly string disabledFolder;
        private readonly Dictionary<string, bool> modStatusCache;

        public ModEnableDisableManager()
        {
            dataFolder = Settings.Default.datafolder;
            disabledFolder = Path.Combine(dataFolder, "disabled_mods");
            modStatusCache = new Dictionary<string, bool>();

            if (!Directory.Exists(disabledFolder))
            {
                Directory.CreateDirectory(disabledFolder);
            }
        }

        public bool IsModEnabled(string modName, string modType)
        {
            var cacheKey = $"{modType}_{modName}";
            
            if (modStatusCache.ContainsKey(cacheKey))
                return modStatusCache[cacheKey];

            var isEnabled = !IsModDisabled(modName, modType);
            modStatusCache[cacheKey] = isEnabled;
            return isEnabled;
        }

        private bool IsModDisabled(string modName, string modType)
        {
            var disabledPath = Path.Combine(disabledFolder, $"{modType}_{modName}");
            return Directory.Exists(disabledPath);
        }

        public bool DisableMod(string modName, string modType, ListView lvMods)
        {
            try
            {
                var modFiles = GetModFiles(modName, modType);
                if (modFiles.Count == 0)
                {
                    MessageBox.Show($"No files found for mod: {modName}", "Disable Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                var disabledPath = Path.Combine(disabledFolder, $"{modType}_{modName}");
                
                // Create disabled directory
                if (!Directory.Exists(disabledPath))
                {
                    Directory.CreateDirectory(disabledPath);
                }

                // Move mod files to disabled folder
                foreach (var file in modFiles)
                {
                    if (File.Exists(file))
                    {
                        var fileName = Path.GetFileName(file);
                        var disabledFilePath = Path.Combine(disabledPath, fileName);
                        
                        // Ensure unique filename
                        var counter = 1;
                        while (File.Exists(disabledFilePath))
                        {
                            var nameWithoutExt = Path.GetFileNameWithoutExtension(fileName);
                            var extension = Path.GetExtension(fileName);
                            disabledFilePath = Path.Combine(disabledPath, $"{nameWithoutExt}_{counter}{extension}");
                            counter++;
                        }

                        File.Move(file, disabledFilePath);
                    }
                }

                // Update cache
                var cacheKey = $"{modType}_{modName}";
                modStatusCache[cacheKey] = false;

                // Update ListView appearance
                UpdateListViewItem(lvMods, modName, modType, false);

                MessageBox.Show($"Mod '{modName}' has been disabled.", "Mod Disabled", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to disable mod: {ex.Message}", "Disable Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool EnableMod(string modName, string modType, ListView lvMods)
        {
            try
            {
                var disabledPath = Path.Combine(disabledFolder, $"{modType}_{modName}");
                if (!Directory.Exists(disabledPath))
                {
                    MessageBox.Show($"No disabled files found for mod: {modName}", "Enable Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                var disabledFiles = Directory.GetFiles(disabledPath, "*", SearchOption.AllDirectories);
                if (disabledFiles.Length == 0)
                {
                    MessageBox.Show($"No files found in disabled folder for mod: {modName}", "Enable Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                // Restore mod files to their original locations
                foreach (var disabledFile in disabledFiles)
                {
                    var relativePath = GetRelativePath(disabledPath, disabledFile);
                    var originalPath = Path.Combine(dataFolder, relativePath);

                    // Ensure directory exists
                    var originalDir = Path.GetDirectoryName(originalPath);
                    if (!Directory.Exists(originalDir))
                    {
                        Directory.CreateDirectory(originalDir);
                    }

                    // Move file back
                    if (File.Exists(originalPath))
                    {
                        File.Delete(originalPath);
                    }
                    File.Move(disabledFile, originalPath);
                }

                // Remove disabled directory
                Directory.Delete(disabledPath, true);

                // Update cache
                var cacheKey = $"{modType}_{modName}";
                modStatusCache[cacheKey] = true;

                // Update ListView appearance
                UpdateListViewItem(lvMods, modName, modType, true);

                MessageBox.Show($"Mod '{modName}' has been enabled.", "Mod Enabled", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to enable mod: {ex.Message}", "Enable Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private List<string> GetModFiles(string modName, string modType)
        {
            var files = new List<string>();
            var installedDir = Path.Combine(dataFolder, "installed");
            var modFile = Path.Combine(installedDir, $"{modType}_{modName}.txt");

            if (File.Exists(modFile))
            {
                files.AddRange(File.ReadAllLines(modFile));
            }

            return files;
        }

        private void UpdateListViewItem(ListView lvMods, string modName, string modType, bool isEnabled)
        {
            foreach (ListViewItem item in lvMods.Items)
            {
                if (item.SubItems[0].Text == modName && item.SubItems[1].Text == modType)
                {
                    if (isEnabled)
                    {
                        item.ForeColor = System.Drawing.Color.Black;
                        item.BackColor = System.Drawing.Color.White;
                    }
                    else
                    {
                        item.ForeColor = System.Drawing.Color.Gray;
                        item.BackColor = System.Drawing.Color.LightGray;
                    }
                    break;
                }
            }
        }

        public List<string> GetDisabledMods()
        {
            var disabledMods = new List<string>();

            if (!Directory.Exists(disabledFolder))
                return disabledMods;

            try
            {
                foreach (var dir in Directory.GetDirectories(disabledFolder))
                {
                    var dirName = Path.GetFileName(dir);
                    disabledMods.Add(dirName);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error reading disabled mods: {ex.Message}");
            }

            return disabledMods;
        }

        public void RefreshModStatus(ListView lvMods)
        {
            modStatusCache.Clear();

            foreach (ListViewItem item in lvMods.Items)
            {
                var modName = item.SubItems[0].Text;
                var modType = item.SubItems[1].Text;
                var isEnabled = IsModEnabled(modName, modType);

                if (isEnabled)
                {
                    item.ForeColor = System.Drawing.Color.Black;
                    item.BackColor = System.Drawing.Color.White;
                }
                else
                {
                    item.ForeColor = System.Drawing.Color.Gray;
                    item.BackColor = System.Drawing.Color.LightGray;
                }
            }
        }

        public bool ToggleMod(string modName, string modType, ListView lvMods)
        {
            if (IsModEnabled(modName, modType))
            {
                return DisableMod(modName, modType, lvMods);
            }
            else
            {
                return EnableMod(modName, modType, lvMods);
            }
        }

        public void EnableAllMods(ListView lvMods)
        {
            var disabledMods = GetDisabledMods();
            var enabledCount = 0;

            foreach (var disabledMod in disabledMods)
            {
                var parts = disabledMod.Split('_');
                if (parts.Length >= 2)
                {
                    var modType = parts[0];
                    var modName = string.Join("_", parts.Skip(1));

                    if (EnableMod(modName, modType, lvMods))
                    {
                        enabledCount++;
                    }
                }
            }

            if (enabledCount > 0)
            {
                MessageBox.Show($"Enabled {enabledCount} mod(s).", "Bulk Enable Complete", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void DisableAllMods(ListView lvMods)
        {
            var result = MessageBox.Show("This will disable all currently enabled mods. Continue?", 
                "Confirm Bulk Disable", MessageBoxButtons.YesNo, MessageBoxIcon.Question);

            if (result != DialogResult.Yes)
                return;

            var disabledCount = 0;

            foreach (ListViewItem item in lvMods.Items)
            {
                var modName = item.SubItems[0].Text;
                var modType = item.SubItems[1].Text;

                if (IsModEnabled(modName, modType))
                {
                    if (DisableMod(modName, modType, lvMods))
                    {
                        disabledCount++;
                    }
                }
            }

            if (disabledCount > 0)
            {
                MessageBox.Show($"Disabled {disabledCount} mod(s).", "Bulk Disable Complete", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        public void CleanupDisabledMods()
        {
            var result = MessageBox.Show("This will permanently delete all disabled mod files. Continue?", 
                "Confirm Cleanup", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            if (result != DialogResult.Yes)
                return;

            try
            {
                if (Directory.Exists(disabledFolder))
                {
                    Directory.Delete(disabledFolder, true);
                    Directory.CreateDirectory(disabledFolder);
                }

                modStatusCache.Clear();

                MessageBox.Show("Disabled mods have been cleaned up.", "Cleanup Complete", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to cleanup disabled mods: {ex.Message}", "Cleanup Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private string GetRelativePath(string basePath, string fullPath)
        {
            if (!fullPath.StartsWith(basePath))
                return fullPath;

            return fullPath.Substring(basePath.Length).TrimStart('\\', '/');
        }
    }
} 