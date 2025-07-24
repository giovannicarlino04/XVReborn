using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using XVReborn.Properties;

namespace XVReborn
{
    public class FileManager
    {
        public List<string> EnumerateFiles(string directory)
        {
            var files = new List<string>();
            foreach (string file in Directory.GetFiles(directory, "*", SearchOption.AllDirectories))
            {
                files.Add(file);
            }
            return files;
        }

        public void CleanupTempFiles()
        {
            var filesToDelete = new[]
            {
                Properties.Settings.Default.datafolder + "//modinfo.xml",
                Properties.Settings.Default.datafolder + @"\system\aura_setting.aur.xml",
                Properties.Settings.Default.datafolder + @"\system\aura_setting.aur.xml.bak",
                Properties.Settings.Default.datafolder + @"\system\custom_skill.cus.xml",
                Properties.Settings.Default.datafolder + @"\system\custom_skill.cus.xml.bak",
                Properties.Settings.Default.datafolder + @"\system\char_model_spec.cms.xml",
                Properties.Settings.Default.datafolder + @"\system\char_model_spec.cms.xml.bak",
                Properties.Settings.Default.datafolder + @"\system\parameter_spec_char.psc.xml",
                Properties.Settings.Default.datafolder + @"\system\parameter_spec_char.psc.xml.bak",
                Properties.Settings.Default.datafolder + @"\system\chara_sound.cso.xml",
                Properties.Settings.Default.datafolder + @"\system\chara_sound.cso.xml.bak",
                Settings.Default.datafolder + "\\x2m.xml"
            };

            foreach (var file in filesToDelete)
            {
                if (File.Exists(file))
                {
                    File.Delete(file);
                }
            }

            var directoriesToDelete = new[]
            {
                Properties.Settings.Default.datafolder + "//temp",
                "./XVRebornTemp"
            };

            foreach (var dir in directoriesToDelete)
            {
                if (Directory.Exists(dir))
                {
                    Directory.Delete(dir, true);
                }
            }
        }

        public void RemoveEmptyDirectories(string path)
        {
            if (!Directory.Exists(path)) return;

            foreach (var subdirectory in Directory.GetDirectories(path))
            {
                RemoveEmptyDirectories(subdirectory);
            }

            if (Directory.GetFileSystemEntries(path).Length == 0)
            {
                try
                {
                    Directory.Delete(path);
                    Console.WriteLine($"Removed empty directory: {path}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error removing directory {path}: {ex.Message}");
                }
            }
        }

        public void MergeDirectoriesWithConfirmation(string sourceDir, string destDir)
        {
            if (!Directory.Exists(sourceDir) || !Directory.Exists(destDir))
            {
                throw new DirectoryNotFoundException("Source or destination directory does not exist.");
            }

            // Copy files
            foreach (string sourceFile in Directory.GetFiles(sourceDir))
            {
                string fileName = Path.GetFileName(sourceFile);
                string destFile = Path.Combine(destDir, fileName);

                if (File.Exists(destFile))
                {
                    var result = MessageBox.Show(
                        $"A file with the name '{fileName}' already exists. Do you want to replace it?", 
                        "File Replace Confirmation", 
                        MessageBoxButtons.YesNoCancel, 
                        MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        File.Copy(sourceFile, destFile, true);
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        return;
                    }
                }
                else
                {
                    File.Copy(sourceFile, destFile);
                }
            }

            // Recursively merge subdirectories
            foreach (string sourceSubDir in Directory.GetDirectories(sourceDir))
            {
                string dirName = Path.GetFileName(sourceSubDir);
                string destSubDir = Path.Combine(destDir, dirName);

                if (!Directory.Exists(destSubDir))
                {
                    Directory.CreateDirectory(destSubDir);
                }

                MergeDirectoriesWithConfirmation(sourceSubDir, destSubDir);
            }
        }

        public void CopyDirectory(string sourceDir, string destDir, bool recursive = true)
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
                var directory = Path.GetDirectoryName(destFile);
            if (!string.IsNullOrEmpty(directory))
                Directory.CreateDirectory(directory);
                File.Copy(file, destFile, true);
            }
        }

        public void MoveDirectory(string sourceDir, string destDir, bool recursive)
        {
            if (!Directory.Exists(sourceDir))
            {
                throw new DirectoryNotFoundException($"Source directory not found: {sourceDir}");
            }

            if (Directory.Exists(destDir))
            {
                Directory.Delete(destDir, recursive);
            }

            Directory.CreateDirectory(destDir);

            foreach (string file in Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories))
            {
                string destFile = file.Replace(sourceDir, destDir);
                var directory = Path.GetDirectoryName(destFile);
            if (!string.IsNullOrEmpty(directory))
                Directory.CreateDirectory(directory);
                File.Move(file, destFile);
            }

            Directory.Delete(sourceDir, recursive);
        }

        public string GetModFilePath(string modType, string modName, string additionalInfo = "")
        {
            var installedDir = Path.Combine(Settings.Default.datafolder, "installed");
            var fileName = string.IsNullOrEmpty(additionalInfo) 
                ? $"{modType}_{modName}.txt" 
                : $"{modType}_{modName}_{additionalInfo}.txt";
            
            return Path.Combine(installedDir, fileName);
        }

        public string[] FindModFiles(string pattern)
        {
            var installedDir = Path.Combine(Settings.Default.datafolder, "installed");
            if (!Directory.Exists(installedDir))
                return new string[0];

            return Directory.GetFiles(installedDir, pattern);
        }

        public void DeleteModFiles(string[] filePaths)
        {
            foreach (string filePath in filePaths)
            {
                if (File.Exists(filePath))
                {
                    var installedFiles = File.ReadAllLines(filePath);
                    foreach (string file in installedFiles)
                    {
                        if (File.Exists(file))
                        {
                            File.Delete(file);
                            Console.WriteLine($"{file} - File deleted");
                        }
                        else if (Directory.Exists(file))
                        {
                            Directory.Delete(file, true);
                            Console.WriteLine($"{file} - Directory deleted");
                        }
                    }
                    File.Delete(filePath);
                }
            }
        }

        public string ExtractModInfoFromFileName(string fileName, string pattern, int groupIndex)
        {
            var match = Regex.Match(fileName, pattern);
            return match.Success ? match.Groups[groupIndex].Value : "";
        }
    }
} 
