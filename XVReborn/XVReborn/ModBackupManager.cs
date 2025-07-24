using System;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows.Forms;
using XVReborn.Properties;

namespace XVReborn
{
    public class ModBackupManager
    {
        private string backupFolder;
        private string dataFolder;

        public ModBackupManager()
        {
            // Don't initialize paths in constructor - they might not be loaded yet
        }

        private void EnsureInitialized()
        {
            if (string.IsNullOrEmpty(dataFolder))
            {
                dataFolder = Settings.Default.datafolder;
                backupFolder = Path.Combine(dataFolder, "backups");
                
                if (!Directory.Exists(backupFolder))
                {
                    Directory.CreateDirectory(backupFolder);
                }
            }
        }

        public bool CreateBackup(string backupName = null)
        {
            try
            {
                EnsureInitialized();
                
                // Validate data folder
                if (string.IsNullOrEmpty(dataFolder))
                {
                    MessageBox.Show("Data folder path is not set. Please configure the data folder in settings.", "Backup Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                if (!Directory.Exists(dataFolder))
                {
                    MessageBox.Show($"Data folder does not exist: {dataFolder}", "Backup Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                // Ensure backup folder exists
                if (!Directory.Exists(backupFolder))
                {
                    try
                    {
                        Directory.CreateDirectory(backupFolder);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Failed to create backup folder: {ex.Message}", "Backup Error", 
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return false;
                    }
                }

                var timestamp = DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
                var backupFileName = backupName ?? $"backup_{timestamp}";
                var backupPath = Path.Combine(backupFolder, $"{backupFileName}.zip");

                // Create backup of all game files except the backup folder itself
                var tempBackupPath = Path.Combine(Path.GetTempPath(), $"temp_backup_{timestamp}");
                
                // Ensure temp directory doesn't exist
                if (Directory.Exists(tempBackupPath))
                {
                    Directory.Delete(tempBackupPath, true);
                }

                try
                {
                    // Copy data folder to temp location, excluding backup folder
                    CopyDirectoryExcludingBackups(dataFolder, tempBackupPath);
                    
                    // Create zip from temp location
                    ZipFile.CreateFromDirectory(tempBackupPath, backupPath, 
                        CompressionLevel.Fastest, false);
                }
                finally
                {
                    // Clean up temp directory
                    if (Directory.Exists(tempBackupPath))
                    {
                        try
                        {
                            Directory.Delete(tempBackupPath, true);
                        }
                        catch
                        {
                            // Ignore cleanup errors
                        }
                    }
                }

                MessageBox.Show($"Backup created successfully: {backupFileName}", "Backup Complete", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to create backup: {ex.Message}\n\nData folder: {dataFolder}\nBackup folder: {backupFolder}", "Backup Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public bool RestoreBackup(string backupPath)
        {
            try
            {
                EnsureInitialized();
                
                if (!File.Exists(backupPath))
                {
                    MessageBox.Show("Backup file not found.", "Restore Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }

                var result = MessageBox.Show(
                    "This will overwrite all current game files. Are you sure you want to continue?",
                    "Confirm Restore", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

                if (result != DialogResult.Yes)
                    return false;

                // Extract backup to temp location first
                var tempExtractPath = Path.Combine(Path.GetTempPath(), $"restore_{DateTime.Now:yyyyMMdd_HHmmss}");
                try
                {
                    ZipFile.ExtractToDirectory(backupPath, tempExtractPath);
                    
                    // Copy files from temp location to data folder, excluding backups
                    if (Directory.Exists(dataFolder))
                    {
                        // Remove existing files except backups folder
                        CleanDataFolderExceptBackups();
                    }
                    
                    // Copy restored files
                    CopyDirectoryExcludingBackups(tempExtractPath, dataFolder);
                }
                finally
                {
                    // Clean up temp directory
                    if (Directory.Exists(tempExtractPath))
                        Directory.Delete(tempExtractPath, true);
                }

                MessageBox.Show("Backup restored successfully.", "Restore Complete", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to restore backup: {ex.Message}", "Restore Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        public string[] GetAvailableBackups()
        {
            EnsureInitialized();
            
            if (!Directory.Exists(backupFolder))
                return new string[0];

            return Directory.GetFiles(backupFolder, "*.zip")
                .Select(Path.GetFileNameWithoutExtension)
                .ToArray();
        }

        private void CopyDirectoryExcludingBackups(string sourceDir, string targetDir)
        {
            try
            {
                var dir = new DirectoryInfo(sourceDir);
                
                // Ensure target directory exists
                if (!Directory.Exists(targetDir))
                {
                    Directory.CreateDirectory(targetDir);
                }

                // Copy files
                foreach (var file in dir.GetFiles())
                {
                    try
                    {
                        var targetPath = Path.Combine(targetDir, file.Name);
                        file.CopyTo(targetPath, true); // Overwrite if exists
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Failed to copy file {file.Name}: {ex.Message}");
                        // Continue with other files
                    }
                }

                // Copy subdirectories, excluding backups folder
                foreach (var subDir in dir.GetDirectories())
                {
                    if (subDir.Name.ToLower() != "backups")
                    {
                        try
                        {
                            var targetSubDir = Path.Combine(targetDir, subDir.Name);
                            CopyDirectoryExcludingBackups(subDir.FullName, targetSubDir);
                        }
                        catch (Exception ex)
                        {
                            System.Diagnostics.Debug.WriteLine($"Failed to copy directory {subDir.Name}: {ex.Message}");
                            // Continue with other directories
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to copy directory from {sourceDir} to {targetDir}: {ex.Message}", ex);
            }
        }

        private void CleanDataFolderExceptBackups()
        {
            var dir = new DirectoryInfo(dataFolder);
            
            // Remove files
            foreach (var file in dir.GetFiles())
            {
                try
                {
                    file.Delete();
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine($"Failed to delete file {file.Name}: {ex.Message}");
                }
            }

            // Remove subdirectories except backups
            foreach (var subDir in dir.GetDirectories())
            {
                if (subDir.Name.ToLower() != "backups")
                {
                    try
                    {
                        subDir.Delete(true);
                    }
                    catch (Exception ex)
                    {
                        System.Diagnostics.Debug.WriteLine($"Failed to delete directory {subDir.Name}: {ex.Message}");
                    }
                }
            }
        }

        public void CleanOldBackups(int keepCount = 5)
        {
            try
            {
                EnsureInitialized();
                
                var backups = Directory.GetFiles(backupFolder, "*.zip")
                    .OrderByDescending(f => File.GetCreationTime(f))
                    .Skip(keepCount)
                    .ToArray();

                foreach (var backup in backups)
                {
                    File.Delete(backup);
                }
            }
            catch (Exception ex)
            {
                // Silently fail for cleanup operations
                System.Diagnostics.Debug.WriteLine($"Failed to clean old backups: {ex.Message}");
            }
        }
    }
} 