using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml;
using XVReborn.Properties;

namespace XVReborn
{
    public class ModDependency
    {
        public string ModName { get; set; }
        public string ModVersion { get; set; }
        public string ModAuthor { get; set; }
        public bool IsRequired { get; set; } = true;
        public string Description { get; set; }
    }

    public class ModDependencyManager
    {
        private readonly string dataFolder;
        private readonly Dictionary<string, List<ModDependency>> modDependencies;

        public ModDependencyManager()
        {
            dataFolder = Settings.Default.datafolder;
            modDependencies = new Dictionary<string, List<ModDependency>>();
        }

        public List<ModDependency> GetModDependencies(string modFilePath)
        {
            var dependencies = new List<ModDependency>();

            try
            {
                var tempDir = Path.Combine(Path.GetTempPath(), "XVReborn_DependencyCheck");
                if (Directory.Exists(tempDir))
                    Directory.Delete(tempDir, true);

                Directory.CreateDirectory(tempDir);
                System.IO.Compression.ZipFile.ExtractToDirectory(modFilePath, tempDir);

                var xmlPath = Path.Combine(tempDir, "xvmod.xml");
                if (File.Exists(xmlPath))
                {
                    dependencies = ParseDependenciesFromXml(xmlPath);
                }

                Directory.Delete(tempDir, true);
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error reading mod dependencies: {ex.Message}");
            }

            return dependencies;
        }

        private List<ModDependency> ParseDependenciesFromXml(string xmlPath)
        {
            var dependencies = new List<ModDependency>();

            try
            {
                var doc = new XmlDocument();
                doc.Load(xmlPath);

                var root = doc.DocumentElement;
                if (root == null) return dependencies;

                // Look for dependencies section
                var dependenciesNode = root.SelectSingleNode("dependencies");
                if (dependenciesNode == null) return dependencies;

                foreach (XmlNode depNode in dependenciesNode.SelectNodes("dependency"))
                {
                    var dependency = new ModDependency
                    {
                        ModName = GetXmlAttribute(depNode, "name"),
                        ModVersion = GetXmlAttribute(depNode, "version"),
                        ModAuthor = GetXmlAttribute(depNode, "author"),
                        IsRequired = GetXmlAttribute(depNode, "required")?.ToLower() != "false",
                        Description = depNode.InnerText?.Trim()
                    };

                    if (!string.IsNullOrEmpty(dependency.ModName))
                    {
                        dependencies.Add(dependency);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error parsing dependencies XML: {ex.Message}");
            }

            return dependencies;
        }

        private string GetXmlAttribute(XmlNode node, string attributeName)
        {
            var attribute = node.Attributes?[attributeName];
            return attribute?.Value?.Trim();
        }

        public List<ModDependency> CheckDependencyStatus(List<ModDependency> dependencies)
        {
            var status = new List<ModDependency>();
            var installedMods = GetInstalledMods();

            foreach (var dependency in dependencies)
            {
                var installedMod = installedMods.FirstOrDefault(m => 
                    m.ModName.ToLower().Equals(dependency.ModName.ToLower()));

                if (installedMod != null)
                {
                    // Check version compatibility
                    if (!string.IsNullOrEmpty(dependency.ModVersion))
                    {
                        if (!IsVersionCompatible(installedMod.ModVersion, dependency.ModVersion))
                        {
                            dependency.Description = $"Version mismatch. Required: {dependency.ModVersion}, Installed: {installedMod.ModVersion}";
                        }
                    }
                }
                else if (dependency.IsRequired)
                {
                    dependency.Description = "Required dependency not installed";
                }

                status.Add(dependency);
            }

            return status;
        }

        private List<ModInfo> GetInstalledMods()
        {
            var installedMods = new List<ModInfo>();
            var installedDir = Path.Combine(dataFolder, "installed");

            if (!Directory.Exists(installedDir))
                return installedMods;

            try
            {
                foreach (var file in Directory.GetFiles(installedDir, "*.txt"))
                {
                    var modInfo = ParseModInfoFromFile(file);
                    if (modInfo != null)
                    {
                        installedMods.Add(modInfo);
                    }
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error reading installed mods: {ex.Message}");
            }

            return installedMods;
        }

        private ModInfo ParseModInfoFromFile(string filePath)
        {
            try
            {
                var fileName = Path.GetFileNameWithoutExtension(filePath);
                var parts = fileName.Split('_');

                if (parts.Length >= 2)
                {
                    return new ModInfo
                    {
                        ModName = parts[1],
                        ModType = parts[0],
                        ModVersion = "Unknown", // Would need to parse from actual mod files
                        ModAuthor = "Unknown"
                    };
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error parsing mod info: {ex.Message}");
            }

            return null;
        }

        private bool IsVersionCompatible(string installedVersion, string requiredVersion)
        {
            try
            {
                // Simple version comparison - can be enhanced for semantic versioning
                return string.Equals(installedVersion?.ToLower(), requiredVersion?.ToLower());
            }
            catch
            {
                return false;
            }
        }

        public bool ShowDependencyDialog(List<ModDependency> dependencies, string modName)
        {
            if (dependencies.Count == 0)
                return true;

            var missingRequired = dependencies.Where(d => d.IsRequired && 
                !IsDependencySatisfied(d)).ToList();

            if (missingRequired.Count == 0)
            {
                var message = $"Mod '{modName}' has the following optional dependencies:\n\n";
                foreach (var dep in dependencies.Where(d => !d.IsRequired))
                {
                    message += $"• {dep.ModName}";
                    if (!string.IsNullOrEmpty(dep.ModVersion))
                        message += $" (v{dep.ModVersion})";
                    if (!string.IsNullOrEmpty(dep.Description))
                        message += $" - {dep.Description}";
                    message += "\n";
                }
                message += "\nThese are optional and the mod should work without them.";

                MessageBox.Show(message, "Optional Dependencies", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
                return true;
            }

            var errorMessage = $"Mod '{modName}' requires the following dependencies:\n\n";
            foreach (var dep in missingRequired)
            {
                errorMessage += $"• {dep.ModName}";
                if (!string.IsNullOrEmpty(dep.ModVersion))
                    errorMessage += $" (v{dep.ModVersion})";
                if (!string.IsNullOrEmpty(dep.Description))
                    errorMessage += $" - {dep.Description}";
                errorMessage += "\n";
            }
            errorMessage += "\nPlease install these dependencies before installing this mod.";

            MessageBox.Show(errorMessage, "Missing Dependencies", 
                MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return false;
        }

        private bool IsDependencySatisfied(ModDependency dependency)
        {
            var installedMods = GetInstalledMods();
            var installedMod = installedMods.FirstOrDefault(m => 
                m.ModName.ToLower().Equals(dependency.ModName.ToLower()));

            if (installedMod == null)
                return false;

            if (!string.IsNullOrEmpty(dependency.ModVersion))
            {
                return IsVersionCompatible(installedMod.ModVersion, dependency.ModVersion);
            }

            return true;
        }

        public void CacheModDependencies(string modName, List<ModDependency> dependencies)
        {
            modDependencies[modName] = dependencies;
        }

        public List<ModDependency> GetCachedDependencies(string modName)
        {
            return modDependencies.ContainsKey(modName) ? modDependencies[modName] : new List<ModDependency>();
        }

        public void ClearCache()
        {
            modDependencies.Clear();
        }
    }

    public class ModInfo
    {
        public string ModName { get; set; }
        public string ModType { get; set; }
        public string ModVersion { get; set; }
        public string ModAuthor { get; set; }
    }
} 