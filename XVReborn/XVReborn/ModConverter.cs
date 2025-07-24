using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using XVReborn.Properties;
using XVReborn.Shared;
using XVReborn.Shared.XV;

namespace XVReborn
{
    public class ModConverter
    {
        private readonly FileManager fileManager;

        public ModConverter()
        {
            fileManager = new FileManager();
        }

        public void ConvertX2MToXVMod()
        {
            var ofd = new OpenFileDialog
            {
                Title = "Convert X2M Mod",
                Filter = "Xenoverse 2 Mod Files (*.x2m)|*.x2m",
                Multiselect = false,
                CheckFileExists = true
            };

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in ofd.FileNames)
                {
                    ProcessX2MFile(file);
                }
            }
        }

        public void ConvertLooseFilesToMod()
        {
            var fbd = new FolderBrowserDialog
            {
                Description = "Convert Loose Files Mod"
            };

            if (fbd.ShowDialog() == DialogResult.OK)
            {
                ProcessLooseFiles(fbd.SelectedPath);
            }
        }

        private void ProcessX2MFile(string filePath)
        {
            var tempFolder = "./XVRebornTemp";
            Directory.CreateDirectory(tempFolder);
            ZipFile.ExtractToDirectory(filePath, tempFolder);

            var x2mFolder = Directory.GetDirectories(tempFolder)
                .FirstOrDefault(d => Regex.IsMatch(Path.GetFileName(d), "^[A-Z0-9]{3}$"));

            if (x2mFolder != null)
            {
                ProcessX2MFiles(x2mFolder);
            }

            var embpackPath = Path.Combine(Settings.Default.datafolder, @"ui\texture", "embpack.exe");
            var finalPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + 
                           $"\\{Path.GetFileNameWithoutExtension(filePath)}";
            
            fileManager.MoveDirectory(tempFolder, finalPath, true);

            var xmlContent = File.ReadAllText(finalPath + @"/x2m.xml");
            var doc = new XmlDocument();
            doc.LoadXml(xmlContent);

            var x2mType = GetAttributeValue(doc.SelectSingleNode("//X2M"), "type");

            if (x2mType == "NEW_CHARACTER")
            {
                ProcessNewCharacterMod(doc, finalPath, embpackPath);
            }
            else if (x2mType == "NEW_SKILL")
            {
                MessageBox.Show("NEW_SKILL conversion is not yet implemented. Please use the skill creator tool instead.", 
                    "Not Implemented", MessageBoxButtons.OK, MessageBoxIcon.Information);
                return;
            }

            MessageBox.Show($"X2M Converted successfully, you can find the converted file in \"{finalPath + ".xvmod"}\"", 
                "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void ProcessX2MFiles(string x2mFolder)
        {
            var x2m = new X2M();

            foreach (var sfile in Directory.GetFiles(x2mFolder, "*", SearchOption.AllDirectories))
            {
                switch (Path.GetExtension(sfile))
                {
                    case ".bcs":
                        x2m.ProcessBCS(sfile);
                        break;
                    case ".bac":
                    case ".bdm":
                        x2m.ProcessBAC(sfile);
                        break;
                    case ".emd":
                    case ".esk":
                    case ".ean":
                        x2m.ChangeModelVer(sfile);
                        break;
                    case ".emm":
                        x2m.ChangeShaderVer(sfile);
                        x2m.ChangeShaderHeader(sfile);
                        break;
                    case ".dyt.emb":
                        x2m.ChangeImageVer(sfile);
                        break;
                }

                if (File.Exists(sfile + @".xml"))
                    File.Delete(sfile + @".xml");
            }
        }

        private void ProcessNewCharacterMod(XmlDocument doc, string finalPath, string embpackPath)
        {
            var ddsFolder = GetDDSFolder(finalPath);
            ProcessDDSFiles(ddsFolder, embpackPath);

            var modData = ExtractModData(doc);
            CreateXVModXml(finalPath, modData);
            OrganizeCharacterFiles(finalPath, modData);
            CreateXVModArchive(finalPath);
        }

        private string GetDDSFolder(string finalPath)
        {
            var matchingDirectories = Directory.GetDirectories(finalPath)
                .Where(dir => Path.GetFileName(dir).Length == 3)
                .ToList();

            return matchingDirectories.Any() ? matchingDirectories.First() : "";
        }

        private void ProcessDDSFiles(string ddsFolder, string embpackPath)
        {
            if (string.IsNullOrEmpty(ddsFolder) || !Directory.Exists(ddsFolder))
                return;

            var embFiles = Directory.GetFiles(ddsFolder, "*.dyt.emb", SearchOption.AllDirectories);
            
            foreach (string embFile in embFiles)
            {
                RunCommand($"\"{embpackPath}\" \"{embFile}\"");
                var ddsFiles = Directory.GetFiles(ddsFolder, "*.dds", SearchOption.AllDirectories);

                if (ddsFiles.Length == 0) continue;

                var groupedByFolder = GroupDDSFilesByFolder(ddsFiles);
                ProcessDDSGroups(groupedByFolder, embpackPath);
            }
        }

        private Dictionary<string, List<string>> GroupDDSFilesByFolder(string[] ddsFiles)
        {
            var groupedByFolder = new Dictionary<string, List<string>>();

            foreach (string ddsFile in ddsFiles)
            {
                string folder = Path.GetDirectoryName(ddsFile);
                if (!groupedByFolder.ContainsKey(folder))
                {
                    groupedByFolder[folder] = new List<string>();
                }
                groupedByFolder[folder].Add(ddsFile);
            }

            return groupedByFolder;
        }

        private void ProcessDDSGroups(Dictionary<string, List<string>> groupedByFolder, string embpackPath)
        {
            foreach (var group in groupedByFolder)
            {
                string folder = group.Key;
                List<string> ddsFilesInFolder = group.Value;

                foreach (string ddsFile in ddsFilesInFolder)
                {
                    DDS.CleanDDSForXV1(ddsFile, ddsFile);
                }

                if (Directory.Exists(folder))
                {
                    RunCommand($"\"{embpackPath}\" \"{folder}\"");
                    Directory.Delete(folder, true);
                }
            }
        }

        private ModData ExtractModData(XmlDocument doc)
        {
            var modData = new ModData
            {
                ModType = "ADDED_CHARACTER",
                ModName = GetAttributeValue(doc.SelectSingleNode("//MOD_NAME"), "value"),
                ModAuthor = GetAttributeValue(doc.SelectSingleNode("//MOD_AUTHOR"), "value"),
                ModVersion = GetAttributeValue(doc.SelectSingleNode("//MOD_VERSION"), "value"),
                AurId = Convert.ToInt32(GetAttributeValue(doc.SelectSingleNode("//CharacLink"), "idAura"), 16),
                AurGlare = GetAttributeValue(doc.SelectSingleNode("//CharacLink"), "glare") == "true" ? 1 : 0,
                CmsBcs = GetAttributeValue(doc.SelectSingleNode("//Entry/CHARACTER"), "value"),
                CmsEan = GetAttributeValue(doc.SelectSingleNode("//Entry/EAN"), "value"),
                CmsFceEan = GetAttributeValue(doc.SelectSingleNode("//Entry/FCE_EAN"), "value"),
                CmsCamEan = GetAttributeValue(doc.SelectSingleNode("//Entry/CAM_EAN"), "value"),
                CmsBac = GetAttributeValue(doc.SelectSingleNode("//Entry/BAC"), "value"),
                CmsBcm = GetAttributeValue(doc.SelectSingleNode("//Entry/BCM"), "value"),
                CmsBai = GetAttributeValue(doc.SelectSingleNode("//Entry/AI"), "value"),
                Cso1 = GetAttributeValue(doc.SelectSingleNode("//CsoEntry/SE"), "value"),
                Cso2 = GetAttributeValue(doc.SelectSingleNode("//CsoEntry/VOX"), "value"),
                Cso3 = GetAttributeValue(doc.SelectSingleNode("//CsoEntry/AMK"), "value"),
                Cso4 = GetAttributeValue(doc.SelectSingleNode("//CsoEntry/SKILLS"), "value"),
                MsgCharacterName = GetAttributeValue(doc.SelectSingleNode("//CHARA_NAME_EN"), "value"),
                MsgCostumeName = GetAttributeValue(doc.SelectSingleNode("//SlotEntry/COSTUME_NAME_EN"), "value"),
                Vox1 = -1,
                Vox2 = -1
            };

            // Extract skills
            var skillsValue = GetAttributeValue(doc.SelectSingleNode("//SkillSet/SKILLS"), "value");
            var skills = skillsValue.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            
            if (skills.Length >= 7)
            {
                modData.CusSuper1 = skills[0].Trim();
                modData.CusSuper2 = skills[1].Trim();
                modData.CusSuper3 = skills[2].Trim();
                modData.CusSuper4 = skills[3].Trim();
                modData.CusUltimate1 = skills[4].Trim();
                modData.CusUltimate2 = skills[5].Trim();
                modData.CusEvasive = skills[6].Trim();
            }

            // Extract PSC data
            ExtractPSCData(doc, modData);

            return modData;
        }

        private void ExtractPSCData(XmlDocument doc, ModData modData)
        {
            modData.PscCostume = "0";
            modData.PscPreset = "0";
            modData.PscCameraPos = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/CAMERA_POSITION"), "value");
            modData.PscHealth = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/HEALTH"), "value");
            modData.PscKi = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/KI"), "value");
            modData.PscKiRecharge = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/KI_RECHARGE"), "value");
            modData.PscStamina = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/STAMINA"), "value");
            modData.PscStaminaRecharge = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/STAMINA_RECHARGE_GROUND"), "value");
            modData.PscBasicAtkDef = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/BASIC_PHYS_DEFENSE"), "value");
            modData.PscBasicKiDef = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/BASIC_KI_DEFENSE"), "value");
            modData.PscStrikeAtkDef = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/STRIKE_ATK_DEFENSE"), "value");
            modData.PscSuperKiDef = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/SUPER_KI_BLAST_DEFENSE"), "value");
            modData.PscGroundSpeed = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/GROUND_SPEED"), "value");
            modData.PscAirSpeed = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/AIR_SPEED"), "value");
            modData.PscBoostSpeed = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/BOOSTING_SPEED"), "value");
            modData.PscDashSpeed = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/DASH_DISTANCE"), "value");
            modData.PscReinforcementSkill = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/REINF_SKILL_DURATION"), "value");
            modData.PscRevivalHpAmount = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/REVIVAL_HP_AMOUNT"), "value");
            modData.PscRevivalSpeed = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/REVIVING_SPEED"), "value");
        }

        private void CreateXVModXml(string finalPath, ModData modData)
        {
            File.Delete(finalPath + @"/x2m.xml");

            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "    "
            };

            var xmlFilePath = finalPath + @"/xvmod.xml";
            using (var writer = XmlWriter.Create(xmlFilePath, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("XVMOD");
                writer.WriteAttributeString("type", modData.ModType);

                WriteElementWithValue(writer, "MOD_NAME", modData.ModName);
                WriteElementWithValue(writer, "MOD_AUTHOR", modData.ModAuthor);
                WriteElementWithValue(writer, "AUR_ID", modData.AurId.ToString());
                WriteElementWithValue(writer, "AUR_GLARE", modData.AurGlare.ToString());
                WriteElementWithValue(writer, "CMS_BCS", modData.CmsBcs);
                WriteElementWithValue(writer, "CMS_EAN", modData.CmsEan);
                WriteElementWithValue(writer, "CMS_FCE_EAN", modData.CmsFceEan);
                WriteElementWithValue(writer, "CMS_CAM_EAN", modData.CmsCamEan);
                WriteElementWithValue(writer, "CMS_BAC", modData.CmsBac);
                WriteElementWithValue(writer, "CMS_BCM", modData.CmsBcm);
                WriteElementWithValue(writer, "CMS_BAI", modData.CmsBai);
                WriteElementWithValue(writer, "CSO_1", modData.Cso1);
                WriteElementWithValue(writer, "CSO_2", modData.Cso2);
                WriteElementWithValue(writer, "CSO_3", modData.Cso3);
                WriteElementWithValue(writer, "CSO_4", modData.Cso4);
                WriteElementWithValue(writer, "CUS_SUPER_1", modData.CusSuper1);
                WriteElementWithValue(writer, "CUS_SUPER_2", modData.CusSuper2);
                WriteElementWithValue(writer, "CUS_SUPER_3", modData.CusSuper3);
                WriteElementWithValue(writer, "CUS_SUPER_4", modData.CusSuper4);
                WriteElementWithValue(writer, "CUS_ULTIMATE_1", modData.CusUltimate1);
                WriteElementWithValue(writer, "CUS_ULTIMATE_2", modData.CusUltimate2);
                WriteElementWithValue(writer, "CUS_EVASIVE", modData.CusEvasive);
                WriteElementWithValue(writer, "PSC_COSTUME", modData.PscCostume);
                WriteElementWithValue(writer, "PSC_PRESET", modData.PscPreset);
                WriteElementWithValue(writer, "PSC_CAMERA_POSITION", modData.PscCameraPos);
                WriteElementWithValue(writer, "PSC_HEALTH", modData.PscHealth);
                WriteElementWithValue(writer, "PSC_KI", modData.PscKi);
                WriteElementWithValue(writer, "PSC_KI_RECHARGE", modData.PscKiRecharge);
                WriteElementWithValue(writer, "PSC_STAMINA", modData.PscStamina);
                WriteElementWithValue(writer, "PSC_STAMINA_RECHARGE", modData.PscStaminaRecharge);
                WriteElementWithValue(writer, "PSC_BASIC_ATK_DEF", modData.PscBasicAtkDef);
                WriteElementWithValue(writer, "PSC_BASIC_KI_DEF", modData.PscBasicKiDef);
                WriteElementWithValue(writer, "PSC_STRIKE_ATK_DEF", modData.PscStrikeAtkDef);
                WriteElementWithValue(writer, "PSC_SUPER_KI_DEF", modData.PscSuperKiDef);
                WriteElementWithValue(writer, "PSC_GROUND_SPEED", modData.PscGroundSpeed);
                WriteElementWithValue(writer, "PSC_AIR_SPEED", modData.PscAirSpeed);
                WriteElementWithValue(writer, "PSC_BOOST_SPEED", modData.PscBoostSpeed);
                WriteElementWithValue(writer, "PSC_DASH_SPEED", modData.PscDashSpeed);
                WriteElementWithValue(writer, "PSC_REINFORCEMENT_SKILL", modData.PscReinforcementSkill);
                WriteElementWithValue(writer, "PSC_REVIVAL_HP_AMOUNT", modData.PscRevivalHpAmount);
                WriteElementWithValue(writer, "PSC_REVIVING_SPEED", modData.PscRevivalSpeed);
                WriteElementWithValue(writer, "MSG_CHARACTER_NAME", modData.MsgCharacterName);
                WriteElementWithValue(writer, "MSG_COSTUME_NAME", modData.MsgCostumeName);
                WriteElementWithValue(writer, "VOX_1", modData.Vox1.ToString());
                WriteElementWithValue(writer, "VOX_2", modData.Vox2.ToString());

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        private void OrganizeCharacterFiles(string finalPath, ModData modData)
        {
            Directory.CreateDirectory(finalPath + $"/ui/texture/CHARA01/");
            File.Move(finalPath + @"/UI/SEL.DDS", finalPath + $"/ui/texture/CHARA01/{modData.CmsBcs}_000.DDS");
            fileManager.MoveDirectory(finalPath + $"/{modData.CmsBcs}", finalPath + $"/chara/{modData.CmsBcs}", true);
        }

        private void CreateXVModArchive(string finalPath)
        {
            ZipFile.CreateFromDirectory(finalPath, finalPath + ".xvmod");
        }

        private void ProcessLooseFiles(string selectedPath)
        {
            foreach (string file in Directory.GetFiles(selectedPath, "*", SearchOption.AllDirectories))
            {
                var x2m = new X2M();
                switch (Path.GetExtension(file))
                {
                    case ".bcs":
                        x2m.ProcessBCS(file);
                        break;
                    case ".bac":
                    case ".bdm":
                        x2m.ProcessBAC(file);
                        break;
                    case ".emd":
                    case ".esk":
                    case ".ean":
                        x2m.ChangeModelVer(file);
                        break;
                    case ".emm":
                        x2m.ChangeShaderVer(file);
                        x2m.ChangeShaderHeader(file);
                        break;
                    case ".dyt.emb":
                        x2m.ChangeImageVer(file);
                        break;
                }

                if (File.Exists(file + @".xml"))
                    File.Delete(file + @".xml");
            }

            CreateLooseFilesMod(selectedPath);
        }

        private void CreateLooseFilesMod(string selectedPath)
        {
            var sfd = new SaveFileDialog
            {
                Title = "Save Mod File",
                Filter = "Xenoverse Mod Files (*.xvmod)|*.xvmod"
            };

            var name = ShowInputDialog("Enter Mod Name: ");
            var author = ShowInputDialog("Enter Author Name: ");

            if (sfd.ShowDialog() == DialogResult.OK)
            {
                var xmlFilePath = selectedPath + "/xvmod.xml";
                CreateModXml(xmlFilePath, name, author);
                ZipFile.CreateFromDirectory(selectedPath, sfd.FileName);

                if (File.Exists(xmlFilePath))
                    File.Delete(xmlFilePath);

                MessageBox.Show("Mod Created Successfully!", "Success", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }

        private void CreateModXml(string xmlFilePath, string name, string author)
        {
            if (File.Exists(xmlFilePath))
                File.Delete(xmlFilePath);

            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "    "
            };

            using (var writer = XmlWriter.Create(xmlFilePath, settings))
            {
                writer.WriteStartDocument();
                writer.WriteStartElement("XVMOD");
                writer.WriteAttributeString("type", "REPLACER");

                WriteElementWithValue(writer, "MOD_NAME", name);
                WriteElementWithValue(writer, "MOD_AUTHOR", author);
                WriteElementWithValue(writer, "MOD_VERSION", "1.0");

                writer.WriteEndElement();
                writer.WriteEndDocument();
            }
        }

        private string GetAttributeValue(XmlNode node, string attributeName)
        {
            var attribute = node?.Attributes[attributeName];
            return attribute?.Value ?? "N/A";
        }

        private void WriteElementWithValue(XmlWriter writer, string elementName, string value)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (value.StartsWith("\"") && value.EndsWith("\""))
                {
                    if (value.Length >= 2)
                    value = value.Substring(1, value.Length - 2);
                }

                writer.WriteStartElement(elementName);
                writer.WriteAttributeString("value", value);
                writer.WriteEndElement();
            }
            else
            {
                writer.WriteStartElement(elementName);
                writer.WriteAttributeString("value", "");
                writer.WriteEndElement();
            }
        }

        private void RunCommand(string command)
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
                    sw.WriteLine(command);
                }
            }
        }

        private string ShowInputDialog(string prompt, string defaultValue = "")
        {
            using (var inputForm = new InputForm(prompt, defaultValue))
            {
                if (inputForm.ShowDialog() == DialogResult.OK)
                {
                    return inputForm.UserInput;
                }
            }
            return string.Empty;
        }
    }
} 