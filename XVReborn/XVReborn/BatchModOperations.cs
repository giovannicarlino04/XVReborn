using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using XVReborn.Properties;

namespace XVReborn
{
    public class BatchModOperations
    {
        private readonly ModInstaller modInstaller;
        private readonly ModUninstaller modUninstaller;
        private readonly ModBackupManager backupManager;
        private readonly ModConflictDetector conflictDetector;

        public BatchModOperations()
        {
            modInstaller = new ModInstaller();
            modUninstaller = new ModUninstaller();
            backupManager = new ModBackupManager();
            conflictDetector = new ModConflictDetector();
        }

        public async Task<bool> BatchInstallMods(List<string> modFilePaths, string language, ListView lvMods, IProgress<int> progress = null)
        {
            if (modFilePaths == null || modFilePaths.Count == 0)
                return false;

            try
            {
                // Create backup before batch installation
                if (!backupManager.CreateBackup($"batch_install_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}"))
                {
                    var result = MessageBox.Show("Failed to create backup. Continue with installation?", 
                        "Backup Failed", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result != DialogResult.Yes)
                        return false;
                }

                var totalMods = modFilePaths.Count;
                var successCount = 0;
                var failedMods = new List<string>();

                for (int i = 0; i < totalMods; i++)
                {
                    var modPath = modFilePaths[i];
                    progress?.Report((i * 100) / totalMods);

                    try
                    {
                        // Check for conflicts
                        var conflicts = conflictDetector.DetectConflicts(modPath);
                        if (conflicts.Count > 0)
                        {
                            var modName = System.IO.Path.GetFileNameWithoutExtension(modPath);
                            if (!conflictDetector.ShowConflictDialog(conflicts, modName))
                            {
                                failedMods.Add($"{modName} (Skipped due to conflicts)");
                                continue;
                            }
                        }

                        // Install the mod
                        modInstaller.InstallMod(modPath, language, lvMods);
                        successCount++;
                    }
                    catch (Exception ex)
                    {
                        var modName = System.IO.Path.GetFileNameWithoutExtension(modPath);
                        failedMods.Add($"{modName} (Error: {ex.Message})");
                        System.Diagnostics.Debug.WriteLine($"Failed to install {modName}: {ex.Message}");
                    }
                }

                progress?.Report(100);

                // Show results
                var message = $"Batch installation completed.\n\n" +
                             $"Successfully installed: {successCount}/{totalMods} mods";

                if (failedMods.Count > 0)
                {
                    message += $"\n\nFailed mods:\n{string.Join("\n", failedMods)}";
                }

                MessageBox.Show(message, "Batch Installation Complete", 
                    MessageBoxButtons.OK, failedMods.Count > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);

                return successCount > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Batch installation failed: {ex.Message}", "Batch Installation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public async Task<bool> BatchUninstallMods(List<ListViewItem> selectedItems, string language, ListView lvMods, IProgress<int> progress = null)
        {
            if (selectedItems == null || selectedItems.Count == 0)
                return false;

            try
            {
                // Create backup before batch uninstallation
                if (!backupManager.CreateBackup($"batch_uninstall_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}"))
                {
                    var result = MessageBox.Show("Failed to create backup. Continue with uninstallation?", 
                        "Backup Failed", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (result != DialogResult.Yes)
                        return false;
                }

                var totalMods = selectedItems.Count;
                var successCount = 0;
                var failedMods = new List<string>();

                for (int i = 0; i < totalMods; i++)
                {
                    var item = selectedItems[i];
                    progress?.Report((i * 100) / totalMods);

                    try
                    {
                        var modName = item.SubItems[0].Text;
                        var modType = item.SubItems[1].Text;

                        modUninstaller.UninstallMod(modType, modName, language, lvMods);
                        successCount++;
                    }
                    catch (Exception ex)
                    {
                        var modName = item.SubItems[0].Text;
                        failedMods.Add($"{modName} (Error: {ex.Message})");
                        System.Diagnostics.Debug.WriteLine($"Failed to uninstall {modName}: {ex.Message}");
                    }
                }

                progress?.Report(100);

                // Show results
                var message = $"Batch uninstallation completed.\n\n" +
                             $"Successfully uninstalled: {successCount}/{totalMods} mods";

                if (failedMods.Count > 0)
                {
                    message += $"\n\nFailed mods:\n{string.Join("\n", failedMods)}";
                }

                MessageBox.Show(message, "Batch Uninstallation Complete", 
                    MessageBoxButtons.OK, failedMods.Count > 0 ? MessageBoxIcon.Warning : MessageBoxIcon.Information);

                return successCount > 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Batch uninstallation failed: {ex.Message}", "Batch Uninstallation Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public List<string> GetSelectedModFilePaths()
        {
            var openFileDialog = new OpenFileDialog
            {
                Title = "Select Mod Files for Batch Installation",
                Filter = "Mod Files (*.x2m;*.zip)|*.x2m;*.zip|All Files (*.*)|*.*",
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                return openFileDialog.FileNames.ToList();
            }

            return new List<string>();
        }

        public List<ListViewItem> GetSelectedListViewItems(ListView lvMods)
        {
            var selectedItems = new List<ListViewItem>();
            
            foreach (ListViewItem item in lvMods.SelectedItems)
            {
                selectedItems.Add(item);
            }

            return selectedItems;
        }

        public void SelectAllMods(ListView lvMods)
        {
            foreach (ListViewItem item in lvMods.Items)
            {
                item.Selected = true;
            }
        }

        public void DeselectAllMods(ListView lvMods)
        {
            lvMods.SelectedItems.Clear();
        }

        public void InvertSelection(ListView lvMods)
        {
            foreach (ListViewItem item in lvMods.Items)
            {
                item.Selected = !item.Selected;
            }
        }

        public void SelectModsByType(ListView lvMods, string modType)
        {
            DeselectAllMods(lvMods);
            
            foreach (ListViewItem item in lvMods.Items)
            {
                if (item.SubItems.Count > 1 && item.SubItems[1].Text.ToLower().Equals(modType.ToLower()))
                {
                    item.Selected = true;
                }
            }
        }

        public int GetSelectedModCount(ListView lvMods)
        {
            return lvMods.SelectedItems.Count;
        }

        public List<string> GetSelectedModNames(ListView lvMods)
        {
            var modNames = new List<string>();
            
            foreach (ListViewItem item in lvMods.SelectedItems)
            {
                modNames.Add(item.SubItems[0].Text);
            }

            return modNames;
        }
    }
} 