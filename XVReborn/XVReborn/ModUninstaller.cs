using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using XVReborn.Properties;
using XVReborn.Shared;
using XVReborn.Shared.XV;

namespace XVReborn
{
    public class ModUninstaller
    {
        private readonly FileManager fileManager;

        public ModUninstaller()
        {
            fileManager = new FileManager();
        }

        public void UninstallMod(string modType, string modName, string language, ListView lvMods)
        {
            var selectedItem = lvMods.SelectedItems[0];
            var normalizedModType = NormalizeModType(modType);

            switch (normalizedModType)
            {
                case "REPLACER":
                    UninstallReplacerMod(modName, selectedItem, lvMods);
                    break;
                case "ADDED_CHARACTER":
                    UninstallCharacterMod(modName, selectedItem, language, lvMods);
                    break;
                case "ADDED_SKILL":
                    UninstallSkillMod(modName, selectedItem, language, lvMods);
                    break;
                default:
                    MessageBox.Show($"Unknown mod type: {modType}", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
        }

        private string NormalizeModType(string modType)
        {
            return modType switch
            {
                "Replacer" => "REPLACER",
                "Added Character" => "ADDED_CHARACTER",
                "Added Skill" => "ADDED_SKILL",
                _ => modType
            };
        }

        private void UninstallReplacerMod(string modName, ListViewItem selectedItem, ListView lvMods)
        {
            var modFilePath = fileManager.GetModFilePath("REPLACER", modName);
            
            if (File.Exists(modFilePath))
            {
                fileManager.DeleteModFiles(new[] { modFilePath });
            }

            RemoveModFromListView(selectedItem, lvMods);
        }

        private void UninstallCharacterMod(string modName, ListViewItem selectedItem, string language, ListView lvMods)
        {
            var matchingFiles = fileManager.FindModFiles($"ADDED_CHARACTER_{modName}_*_*.txt");
            var modFilePath = matchingFiles.Length > 0 ? matchingFiles[0] : null;

            if (modFilePath != null)
            {
                var fileName = Path.GetFileName(modFilePath);
                var pattern = @"^(\w+)_(.*?)_(\w+)_(\d+)\.txt$";
                var match = Regex.Match(fileName, pattern);

                if (match.Success)
                {
                    var cmsBcs = match.Groups[3].Value;
                    var charId = int.Parse(match.Groups[4].Value);

                    fileManager.DeleteModFiles(new[] { modFilePath });
                    RemoveCharacterFromFiles(cmsBcs, charId, language);
                }
            }

            RemoveModFromListView(selectedItem, lvMods);
        }

        private void UninstallSkillMod(string modName, ListViewItem selectedItem, string language, ListView lvMods)
        {
            var matchingFiles = fileManager.FindModFiles($"ADDED_SKILL_{modName}_*_*_*_*.txt");
            var modFilePath = matchingFiles.Length > 0 ? matchingFiles[0] : null;

            if (modFilePath != null)
            {
                var fileName = Path.GetFileName(modFilePath);
                var pattern = @"^(\w+)_(\w+)_(\w+)_(\d{3})_(\w+)_(\w+)_(.*?)\.txt$";
                var match = Regex.Match(fileName, pattern);

                if (match.Success)
                {
                    var skillType = match.Groups[3].Value;
                    var skillId1 = match.Groups[4].Value;
                    var skillId2 = match.Groups[5].Value;
                    var skillShortName = match.Groups[7].Value;

                    fileManager.DeleteModFiles(new[] { modFilePath });
                    RemoveSkillFromFiles(skillType, skillId1, skillId2, skillShortName, language);
                }
            }

            RemoveModFromListView(selectedItem, lvMods);
        }

        private void RemoveCharacterFromFiles(string cmsBcs, int charId, string language)
        {
            RemoveFromCMS(cmsBcs);
            RemoveFromCSO(charId);
            RemoveFromCUS(charId);
            RemoveFromAUR(charId);
            RemoveFromPSC(charId);
            RemoveFromCharacterList(cmsBcs);
            RemoveFromMessages(cmsBcs, language);
        }

        private void RemoveFromCMS(string cmsBcs)
        {
            var cms = new CMS();
            cms.Load(Settings.Default.datafolder + @"/system/char_model_spec.cms");
            
            for (int i = 0; i < cms.Data.Count(); i++)
            {
                if (cms.Data[i].ShortName == cmsBcs)
                {
                    cms.RemoveCharacter(cms.Data[i]);
                    break;
                }
            }
        }

        private void RemoveFromCSO(int charId)
        {
            var cso = new CSO();
            cso.Load(Settings.Default.datafolder + @"/system/chara_sound.cso");
            
            for (int i = 0; i < cso.Data.Count(); i++)
            {
                if (cso.Data[i].Char_ID == charId)
                {
                    cso.RemoveCharacter(cso.Data[i]);
                    break;
                }
            }
        }

        private void RemoveFromCUS(int charId)
        {
            var serializerPath = Settings.Default.datafolder + "/system/XMLSerializer.exe";
            RunCommand($"\"{serializerPath}\" \"{Settings.Default.datafolder + @"/system/custom_skill.cus"}\"");

            var cusPath = Settings.Default.datafolder + @"/system/custom_skill.cus.xml";
            if (File.Exists(cusPath))
            {
                var text = File.ReadAllText(cusPath);
                var skillsetToRemove = $"    <Skillset Character_ID=\"{charId}\" Costume_Index=\"0\" Model_Preset=\"0\">.*?</Skillset>";
                text = Regex.Replace(text, skillsetToRemove, "", RegexOptions.Singleline);
                File.WriteAllText(cusPath, text);
            }

            RunCommand($"\"{serializerPath}\" \"{cusPath}\"");
        }

        private void RemoveFromAUR(int charId)
        {
            var serializerPath = Settings.Default.datafolder + "/system/XMLSerializer.exe";
            RunCommand($"\"{serializerPath}\" \"{Settings.Default.datafolder + @"/system/aura_setting.aur"}\"");

            var aurPath = Settings.Default.datafolder + @"/system/aura_setting.aur.xml";
            if (File.Exists(aurPath))
            {
                var text = File.ReadAllText(aurPath);
                var auraToRemove = $"    <CharacterAura Chara_ID=\"{charId}\" Costume=\"0\".*?/>\r\n";
                text = Regex.Replace(text, auraToRemove, "");
                File.WriteAllText(aurPath, text);
            }

            RunCommand($"\"{serializerPath}\" \"{aurPath}\"");
        }

        private void RemoveFromPSC(int charId)
        {
            var serializerPath = Settings.Default.datafolder + "/system/XMLSerializer.exe";
            RunCommand($"\"{serializerPath}\" \"{Settings.Default.datafolder + @"/system/parameter_spec_char.psc"}\"");

            var pscPath = Settings.Default.datafolder + @"/system/parameter_spec_char.psc.xml";
            if (File.Exists(pscPath))
            {
                var text = File.ReadAllText(pscPath);
                var pscToRemove = $"    <PSC_Entry Chara_ID=\"{charId}\">.*?</PSC_Entry>";
                text = Regex.Replace(text, pscToRemove, "", RegexOptions.Singleline);
                File.WriteAllText(pscPath, text);
            }

            RunCommand($"\"{serializerPath}\" \"{pscPath}\"");
        }

        private void RemoveFromCharacterList(string cmsBcs)
        {
            var charaList = Settings.Default.datafolder + @"/XVP_SLOTS.xs";
            if (File.Exists(charaList))
            {
                var content = File.ReadAllText(charaList);
                var escapedCmsBcs = Regex.Escape(cmsBcs);
                var pattern = @"\{\[\s*" + escapedCmsBcs + @"\s*,\s*0\s*,\s*0\s*,\s*0\s*,\s*(-?\d{1,3})\s*,\s*(-?\d{1,3})\s*\]\}";
                var modifiedContent = Regex.Replace(content, pattern, "");
                File.WriteAllText(charaList, modifiedContent);
            }
        }

        private void RemoveFromMessages(string cmsBcs, string language)
        {
            var msgPath = Settings.Default.datafolder + @"/msg/proper_noun_character_name_" + language + ".msg";
            if (File.Exists(msgPath))
            {
                var msgFile = msgStream.Load(msgPath);
                
                // Add null and bounds checking
                if (msgFile?.data == null)
                {
                    MessageBox.Show("Error: Invalid message file structure", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var updatedData = msgFile.data.Where(d => !d.NameID.Contains(cmsBcs)).ToArray();
                msgFile.data = updatedData;
                msgStream.Save(msgFile, msgPath);
            }
        }

        private void RemoveSkillFromFiles(string skillType, string skillId1, string skillId2, string skillShortName, string language)
        {
            RemoveFromSkillCUS(skillType, skillId1, skillId2, skillShortName);
            RemoveFromSkillMessages(skillType, skillId1, language);
        }

        private void RemoveFromSkillCUS(string skillType, string skillId1, string skillId2, string skillShortName)
        {
            var serializerPath = Settings.Default.datafolder + "/system/XMLSerializer.exe";
            RunCommand($"\"{serializerPath}\" \"{Settings.Default.datafolder + @"/system/custom_skill.cus"}\"");

            var cusPath = Settings.Default.datafolder + @"\system\custom_skill.cus.xml";
            if (File.Exists(cusPath))
            {
                var xmlContent = File.ReadAllText(cusPath);
                var patternCUS = $@"<Skill ShortName=""{skillShortName}"" ID1=""{skillId1.PadLeft(3, '0')}"" ID2=""{skillId2.PadLeft(3, '0')}"">.*?</Skill>";
                var updatedXmlContent = Regex.Replace(xmlContent, patternCUS, string.Empty, RegexOptions.Singleline);
                File.WriteAllText(cusPath, updatedXmlContent);
            }

            RunCommand($"\"{serializerPath}\" \"{cusPath}\"");
        }

        private void RemoveFromSkillMessages(string skillType, string skillId1, string language)
        {
            var msgPathName = Settings.Default.datafolder + @"/msg/proper_noun_skill_" + skillType.ToLower() + "_name_" + language + ".msg";
            var msgPathInfo = Settings.Default.datafolder + @"/msg/proper_noun_skill_" + skillType.ToLower() + "_info_" + language + ".msg";

            RemoveFromMessageFile(msgPathName, skillId1);
            RemoveFromMessageFile(msgPathInfo, skillId1);
        }

        private void RemoveFromMessageFile(string filePath, string skillId1)
        {
            if (File.Exists(filePath))
            {
                var msgFile = msgStream.Load(filePath);
                
                // Add null and bounds checking
                if (msgFile?.data == null)
                {
                    MessageBox.Show("Error: Invalid message file structure", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                var updatedData = msgFile.data.Where(d => !d.NameID.Contains(skillId1.PadLeft(3, '0'))).ToArray();
                msgFile.data = updatedData;
                msgStream.Save(msgFile, filePath);
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

            process.WaitForExit();
        }

        private void RemoveModFromListView(ListViewItem selectedItem, ListView lvMods)
        {
            lvMods.Items.Remove(selectedItem);
            SaveModList(lvMods);
            MessageBox.Show("Mod successfully uninstalled.", "Success", 
                MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void SaveModList(ListView lvMods)
        {
            Settings.Default.modlist = new StringCollection();
            Settings.Default.modlist.AddRange(
                (from i in lvMods.Items.Cast<ListViewItem>()
                 select string.Join("|", 
                     from si in i.SubItems.Cast<ListViewItem.ListViewSubItem>()
                     select si.Text)).ToArray());
            Settings.Default.Save();
        }
    }
} 