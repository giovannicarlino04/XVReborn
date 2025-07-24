using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Windows.Forms;
using System.Xml;

namespace XVReborn
{
    public class ModValidationResult
    {
        public bool IsValid { get; set; }
        public List<string> Errors { get; set; } = new List<string>();
        public List<string> Warnings { get; set; } = new List<string>();
        public string ModName { get; set; }
        public string ModType { get; set; }
        public string ModAuthor { get; set; }
        public string ModVersion { get; set; }
        public int FileCount { get; set; }
        public long TotalSize { get; set; }
    }

    public class ModValidator
    {
        private readonly string[] requiredFiles = { "xvmod.xml" };
        private readonly string[] validModTypes = { "REPLACER", "ADDED_CHARACTER", "ADDED_SKILL" };

        public ModValidationResult ValidateMod(string modFilePath)
        {
            var result = new ModValidationResult();

            try
            {
                if (!File.Exists(modFilePath))
                {
                    result.Errors.Add("Mod file does not exist.");
                    return result;
                }

                // Check file extension
                var extension = Path.GetExtension(modFilePath).ToLower();
                if (extension != ".xvmod" && extension != ".zip" )
                {
                    result.Errors.Add($"Invalid file format. Expected .xvmod or .zip, got {extension}");
                }

                // Extract and validate mod contents
                var tempDir = Path.Combine(Path.GetTempPath(), "XVReborn_Validation");
                if (Directory.Exists(tempDir))
                    Directory.Delete(tempDir, true);

                Directory.CreateDirectory(tempDir);

                try
                {
                    ZipFile.ExtractToDirectory(modFilePath, tempDir);
                    ValidateModContents(tempDir, result);
                }
                finally
                {
                    // Cleanup
                    if (Directory.Exists(tempDir))
                        Directory.Delete(tempDir, true);
                }

                result.IsValid = result.Errors.Count == 0;
            }
            catch (Exception ex)
            {
                result.Errors.Add($"Validation failed: {ex.Message}");
                result.IsValid = false;
            }

            return result;
        }

        private void ValidateModContents(string modDir, ModValidationResult result)
        {
            // Check for required files
            foreach (var requiredFile in requiredFiles)
            {
                var filePath = Path.Combine(modDir, requiredFile);
                if (!File.Exists(filePath))
                {
                    result.Errors.Add($"Required file missing: {requiredFile}");
                }
            }

            // Validate xvmod.xml
            var xmlPath = Path.Combine(modDir, "xvmod.xml");
            if (File.Exists(xmlPath))
            {
                ValidateModXml(xmlPath, result);
            }

            // Check for empty directories
            var allFiles = Directory.GetFiles(modDir, "*", SearchOption.AllDirectories);
            if (allFiles.Length == 0)
            {
                result.Errors.Add("Mod contains no files.");
            }

            result.FileCount = allFiles.Length;
            result.TotalSize = allFiles.Sum(f => new FileInfo(f).Length);

            // Check for suspicious files
            CheckForSuspiciousFiles(modDir, result);

            // Validate file structure
            ValidateFileStructure(modDir, result);
        }

        private void ValidateModXml(string xmlPath, ModValidationResult result)
        {
            try
            {
                var doc = new XmlDocument();
                doc.Load(xmlPath);

                var root = doc.DocumentElement;
                if (root == null)
                {
                    result.Errors.Add("Invalid XML structure: no root element");
                    return;
                }

                // Extract basic mod information
                result.ModName = GetXmlValue(root, "MOD_NAME") ?? "Unknown";
                result.ModType = root.GetAttribute("type") ?? "Unknown";
                result.ModAuthor = GetXmlValue(root, "MOD_AUTHOR") ?? "Unknown";
                result.ModVersion = GetXmlValue(root, "MOD_VERSION") ?? "Unknown";

                // Validate mod type
                if (!validModTypes.Contains(result.ModType))
                {
                    result.Errors.Add($"Invalid mod type: {result.ModType}. Valid types are: {string.Join(", ", validModTypes)}");
                }

                // Check for required fields based on mod type
                ValidateModTypeSpecificFields(root, result);

                // Check for common issues
                if (string.IsNullOrWhiteSpace(result.ModName))
                {
                    result.Warnings.Add("Mod name is empty or missing");
                }

                if (string.IsNullOrWhiteSpace(result.ModAuthor))
                {
                    result.Warnings.Add("Mod author is empty or missing");
                }

                if (string.IsNullOrWhiteSpace(result.ModVersion))
                {
                    result.Warnings.Add("Mod version is empty or missing");
                }
            }
            catch (XmlException ex)
            {
                result.Errors.Add($"Invalid XML format: {ex.Message}");
            }
            catch (Exception ex)
            {
                result.Errors.Add($"Error parsing XML: {ex.Message}");
            }
        }

        private string GetXmlValue(XmlElement root, string elementName)
        {
            var element = root.SelectSingleNode(elementName);
            if (element != null)
            {
                // Check if it has a value attribute first
                var valueAttr = element.Attributes["value"];
                if (valueAttr != null)
                {
                    return valueAttr.Value?.Trim();
                }
                // Fall back to inner text
                return element.InnerText?.Trim();
            }
            return null;
        }

        private void ValidateModTypeSpecificFields(XmlElement root, ModValidationResult result)
        {
            switch (result.ModType)
            {
                case "ADDED_CHARACTER":
                    ValidateCharacterModFields(root, result);
                    break;
                case "ADDED_SKILL":
                    ValidateSkillModFields(root, result);
                    break;
                case "REPLACER":
                    ValidateReplacerModFields(root, result);
                    break;
            }
        }

        private void ValidateCharacterModFields(XmlElement root, ModValidationResult result)
        {
            var requiredFields = new[] { "CMS_BCS", "CMS_EAN", "CMS_BCM" };
            foreach (var field in requiredFields)
            {
                if (string.IsNullOrWhiteSpace(GetXmlValue(root, field)))
                {
                    result.Warnings.Add($"Character mod field missing: {field}");
                }
            }
        }

        private void ValidateSkillModFields(XmlElement root, ModValidationResult result)
        {
            var requiredFields = new[] { "SKILL_TYPE", "SKILL_SHORT_NAME", "SKILL_ID1" };
            foreach (var field in requiredFields)
            {
                if (string.IsNullOrWhiteSpace(GetXmlValue(root, field)))
                {
                    result.Warnings.Add($"Skill mod field missing: {field}");
                }
            }
        }

        private void ValidateReplacerModFields(XmlElement root, ModValidationResult result)
        {
            // Replacer mods typically don't have specific required fields
            // but we can check for common issues
            var description = GetXmlValue(root, "MOD_DESCRIPTION");
            if (string.IsNullOrWhiteSpace(description))
            {
                result.Warnings.Add("Replacer mod has no description");
            }
        }

        private void CheckForSuspiciousFiles(string modDir, ModValidationResult result)
        {
            var suspiciousExtensions = new[] { ".exe", ".bat", ".cmd", ".ps1", ".vbs" };
            var allFiles = Directory.GetFiles(modDir, "*", SearchOption.AllDirectories);

            foreach (var file in allFiles)
            {
                var extension = Path.GetExtension(file).ToLower();
                if (suspiciousExtensions.Contains(extension))
                {
                    result.Warnings.Add($"Suspicious file detected: {Path.GetFileName(file)}");
                }
            }
        }

        private void ValidateFileStructure(string modDir, ModValidationResult result)
        {
            // Check for common game file directories
            var gameDirs = new[] { "data", "system", "ui", "sound", "texture" };
            var foundDirs = Directory.GetDirectories(modDir, "*", SearchOption.TopDirectoryOnly)
                .Select(Path.GetFileName)
                .ToList();

            if (!foundDirs.Any(d => gameDirs.Contains(d?.ToLower())))
            {
                result.Warnings.Add("No standard game directories found. Mod may not install correctly.");
            }
        }

        public bool ShowValidationDialog(ModValidationResult result)
        {
            if (result.IsValid && result.Warnings.Count == 0)
            {
                MessageBox.Show("Mod validation passed successfully!", "Validation Complete", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }

            var message = $"Mod: {result.ModName}\n";
            message += $"Type: {result.ModType}\n";
            message += $"Author: {result.ModAuthor}\n";
            message += $"Version: {result.ModVersion}\n";
            message += $"Files: {result.FileCount}\n";
            message += $"Size: {FormatFileSize(result.TotalSize)}\n\n";

            if (result.Errors.Count > 0)
            {
                message += "Errors:\n";
                foreach (var error in result.Errors)
                {
                    message += $"• {error}\n";
                }
                message += "\n";
            }

            if (result.Warnings.Count > 0)
            {
                message += "Warnings:\n";
                foreach (var warning in result.Warnings)
                {
                    message += $"• {warning}\n";
                }
                message += "\n";
            }

            if (result.IsValid)
            {
                message += "The mod appears to be valid but has some warnings.";
                var dialogResult = MessageBox.Show(message, "Mod Validation - Warnings", 
                    MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                return dialogResult == DialogResult.Yes;
            }
            else
            {
                message += "The mod has validation errors and may not install correctly.";
                MessageBox.Show(message, "Mod Validation - Errors", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
        }

        private string FormatFileSize(long bytes)
        {
            if (bytes < 1024)
                return $"{bytes} B";
            else if (bytes < 1024 * 1024)
                return $"{bytes / 1024.0:F1} KB";
            else
                return $"{bytes / (1024.0 * 1024.0):F1} MB";
        }

        public List<ModValidationResult> ValidateMultipleMods(List<string> modFilePaths)
        {
            var results = new List<ModValidationResult>();
            
            foreach (var modPath in modFilePaths)
            {
                var result = ValidateMod(modPath);
                results.Add(result);
            }

            return results;
        }
    }
} 