using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using XVReborn.Properties;

namespace XVReborn
{
    public class ModConflict
    {
        public string FilePath { get; set; }
        public List<string> ConflictingMods { get; set; } = new List<string>();
        public string ConflictType { get; set; } // "File Overwrite", "Directory Overwrite"
    }

    public class ModConflictDetector
    {
        private readonly string dataFolder;
        private readonly Dictionary<string, List<string>> installedModFiles;

        public ModConflictDetector()
        {
            dataFolder = Settings.Default.datafolder;
            installedModFiles = new Dictionary<string, List<string>>();
            LoadInstalledModFiles();
        }

        private void LoadInstalledModFiles()
        {
            var installedDir = Path.Combine(dataFolder, "installed");
            if (!Directory.Exists(installedDir))
                return;

            foreach (var file in Directory.GetFiles(installedDir, "*.txt"))
            {
                var modName = Path.GetFileNameWithoutExtension(file);
                var files = File.ReadAllLines(file).ToList();
                installedModFiles[modName] = files;
            }
        }

        public List<ModConflict> DetectConflicts(string newModPath)
        {
            var conflicts = new List<ModConflict>();
            
            try
            {
                // Extract mod temporarily to analyze files
                var tempDir = Path.Combine(Path.GetTempPath(), "XVReborn_ConflictCheck");
                if (Directory.Exists(tempDir))
                    Directory.Delete(tempDir, true);
                
                Directory.CreateDirectory(tempDir);
                System.IO.Compression.ZipFile.ExtractToDirectory(newModPath, tempDir);

                var newModFiles = GetFilesRecursively(tempDir)
                    .Select(f => f.Replace(tempDir, dataFolder))
                    .ToList();

                // Check for conflicts with installed mods
                foreach (var kvp in installedModFiles)
                {
                    var modName = kvp.Key;
                    var modFiles = kvp.Value;

                    foreach (var newFile in newModFiles)
                    {
                        if (modFiles.Contains(newFile))
                        {
                            var conflict = conflicts.FirstOrDefault(c => c.FilePath == newFile);
                            if (conflict == null)
                            {
                                conflict = new ModConflict
                                {
                                    FilePath = newFile,
                                    ConflictType = "File Overwrite"
                                };
                                conflicts.Add(conflict);
                            }
                            conflict.ConflictingMods.Add(modName);
                        }
                    }
                }

                // Cleanup temp directory
                Directory.Delete(tempDir, true);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error detecting conflicts: {ex.Message}", "Conflict Detection Error", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            return conflicts;
        }

        private List<string> GetFilesRecursively(string directory)
        {
            var files = new List<string>();
            
            try
            {
                files.AddRange(Directory.GetFiles(directory));
                
                foreach (var subDir in Directory.GetDirectories(directory))
                {
                    files.AddRange(GetFilesRecursively(subDir));
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error reading directory {directory}: {ex.Message}");
            }

            return files;
        }

        public bool ShowConflictDialog(List<ModConflict> conflicts, string modName)
        {
            if (conflicts.Count == 0)
                return true;

            var message = $"The mod '{modName}' conflicts with {conflicts.Count} file(s) from other installed mods:\n\n";
            
            foreach (var conflict in conflicts.Take(10)) // Show first 10 conflicts
            {
                message += $"• {Path.GetFileName(conflict.FilePath)} (conflicts with: {string.Join(", ", conflict.ConflictingMods)})\n";
            }

            if (conflicts.Count > 10)
                message += $"\n... and {conflicts.Count - 10} more conflicts.";

            message += "\n\nInstalling this mod may overwrite files from other mods. Do you want to continue?";

            var result = MessageBox.Show(message, "Mod Conflicts Detected", 
                MessageBoxButtons.YesNo, MessageBoxIcon.Warning);

            return result == DialogResult.Yes;
        }

        public void UpdateInstalledModFiles(string modName, List<string> files)
        {
            installedModFiles[modName] = files;
        }

        public void RemoveInstalledModFiles(string modName)
        {
            if (installedModFiles.ContainsKey(modName))
                installedModFiles.Remove(modName);
        }
    }
} 