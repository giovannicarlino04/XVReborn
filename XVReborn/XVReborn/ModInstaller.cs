using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using XVReborn.Properties;
using XVReborn.Shared;
using XVReborn.Shared.XV;

namespace XVReborn
{
    public class ModInstaller
    {
        private readonly FileManager fileManager;
        private readonly ModDataParser modDataParser;

        public ModInstaller()
        {
            fileManager = new FileManager();
            modDataParser = new ModDataParser();
        }

        public void InstallMod(string modFilePath, string language, ListView lvMods)
        {
            var xmlFile = "./XVRebornTemp/xvmod.xml";
            var charId = 108 + Settings.Default.modlist.Count;

            CleanupTempFiles();
            ExtractMod(modFilePath);
            
            if (!File.Exists(xmlFile))
            {
                MessageBox.Show("xvmod.xml file not found.", "Invalid mod file", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var modData = modDataParser.ParseModXml(xmlFile);
            
            switch (modData.ModType)
            {
                case "REPLACER":
                    InstallReplacerMod(modData, lvMods);
                    break;
                case "ADDED_CHARACTER":
                    InstallCharacterMod(modData, charId, language, lvMods);
                    break;
                case "ADDED_SKILL":
                    InstallSkillMod(modData, charId, language, lvMods);
                    break;
                default:
                    MessageBox.Show($"Unknown mod type: {modData.ModType}", "Error", 
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
            }
        }

        private void CleanupTempFiles()
        {
            if (File.Exists(Settings.Default.datafolder + "/xvmod.xml"))
                File.Delete(Settings.Default.datafolder + "/xvmod.xml");
            if (File.Exists("./XVRebornTemp/xvmod.xml"))
                File.Delete("./XVRebornTemp/xvmod.xml");
        }

        private void ExtractMod(string modFilePath)
        {
            ZipFile.ExtractToDirectory(modFilePath, "./XVRebornTemp");
        }

        private void InstallReplacerMod(ModData modData, ListView lvMods)
        {
            var installedFiles = fileManager.EnumerateFiles("./XVRebornTemp");
            var finalFiles = installedFiles.Select(file => 
                file.Replace("./XVRebornTemp", Properties.Settings.Default.datafolder)).ToList();

            var installedDir = Path.Combine(Settings.Default.datafolder, "installed");
            if (!Directory.Exists(installedDir))
            {
                Directory.CreateDirectory(installedDir);
            }

            var filePath = Path.Combine(installedDir, $"{modData.ModType}_{modData.ModName}.txt");
            File.WriteAllLines(filePath, finalFiles);

            fileManager.MergeDirectoriesWithConfirmation("./XVRebornTemp", Settings.Default.datafolder);

            AddModToListView(modData, "Replacer", lvMods);
        }

        private void InstallCharacterMod(ModData modData, int charId, string language, ListView lvMods)
        {
            var installedFiles = fileManager.EnumerateFiles("./XVRebornTemp");
            var finalFiles = installedFiles.Select(file => 
                file.Replace("./XVRebornTemp", Properties.Settings.Default.datafolder)).ToList();

            var installedDir = Path.Combine(Settings.Default.datafolder, "installed");
            if (!Directory.Exists(installedDir))
            {
                Directory.CreateDirectory(installedDir);
            }

            var filePath = Path.Combine(installedDir, 
                $"{modData.ModType}_{modData.ModName}_{modData.CmsBcs}_{charId}.txt");
            File.WriteAllLines(filePath, finalFiles);

            if (Directory.Exists("./XVRebornTemp/JUNGLE"))
                fileManager.MergeDirectoriesWithConfirmation("./XVRebornTemp/JUNGLE", Settings.Default.datafolder);
            fileManager.MergeDirectoriesWithConfirmation("./XVRebornTemp", Settings.Default.datafolder);

            UpdateCharacterFiles(modData, charId, language);
            AddModToListView(modData, "Added Character", lvMods);
        }

        private void InstallSkillMod(ModData modData, int charId, string language, ListView lvMods)
        {
            var installedFiles = fileManager.EnumerateFiles("./XVRebornTemp");
            var finalFiles = installedFiles.Select(file => 
                file.Replace("./XVRebornTemp", Properties.Settings.Default.datafolder)).ToList();

            var installedDir = Path.Combine(Settings.Default.datafolder, "installed");
            if (!Directory.Exists(installedDir))
            {
                Directory.CreateDirectory(installedDir);
            }

            var filePath = Path.Combine(installedDir, 
                $"{modData.ModType}_{modData.ModName}_{modData.SkillType}_{modData.SkillId1.PadLeft(3, '0')}_{modData.SkillId2.PadLeft(3, '0')}_{charId}_{modData.SkillShortName}.txt");
            File.WriteAllLines(filePath, finalFiles);

            if (Directory.Exists("./XVRebornTemp/JUNGLE"))
                fileManager.MergeDirectoriesWithConfirmation("./XVRebornTemp/JUNGLE", Settings.Default.datafolder);
            fileManager.MergeDirectoriesWithConfirmation("./XVRebornTemp", Settings.Default.datafolder);

            UpdateSkillFiles(modData, language);
            AddModToListView(modData, "Added Skill", lvMods);
        }

        private void UpdateCharacterFiles(ModData modData, int charId, string language)
        {
            UpdateCMS(modData, charId);
            UpdateCSO(modData, charId);
            UpdateCUS(modData, charId);
            UpdateAUR(modData, charId);
            UpdatePSC(modData, charId);
            UpdateCharacterList(modData, charId);
            UpdateCharacterMessages(modData, charId, language);
            UpdateCharacterTextures();
        }

        private void UpdateSkillFiles(ModData modData, string language)
        {
            UpdateSkillCUS(modData);
            UpdateSkillMessages(modData, language);
        }

        private void UpdateCMS(ModData modData, int charId)
        {
            var cms = new CMS();
            cms.Load(Settings.Default.datafolder + @"/system/char_model_spec.cms");
            
            var newCharacter = new CharacterData
            {
                ID = charId,
                ShortName = modData.CmsBcs,
                Unknown = new byte[8],
                Paths = new string[7]
            };
            
            newCharacter.Paths[0] = modData.CmsBcs;
            newCharacter.Paths[1] = modData.CmsEan;
            newCharacter.Paths[2] = modData.CmsFceEan;
            newCharacter.Paths[3] = modData.CmsCamEan;
            newCharacter.Paths[4] = modData.CmsBac;
            newCharacter.Paths[5] = modData.CmsBcm;
            newCharacter.Paths[6] = modData.CmsBai;
            
            cms.AddCharacter(newCharacter);
        }

        private void UpdateCSO(ModData modData, int charId)
        {
            var cso = new CSO();
            cso.Load(Settings.Default.datafolder + @"/system/chara_sound.cso");
            
            var characterData = new CSO_Data
            {
                Char_ID = charId,
                Costume_ID = 0,
                Paths = new string[4]
                {
                    modData.Cso1,
                    modData.Cso2,
                    modData.Cso3,
                    modData.Cso4
                }
            };
            
            cso.AddCharacter(characterData);
        }

        private void UpdateCUS(ModData modData, int charId)
        {
            RunXMLSerializer("custom_skill.cus");
            
            var cusPath = Settings.Default.datafolder + @"\system\custom_skill.cus.xml";
            var text = File.ReadAllText(cusPath);
            
            var skillsetEntry = $@"    <Skillset Character_ID=""{charId}"" Costume_Index=""0"" Model_Preset=""0"">
      <SuperSkill1 ID1=""{modData.CusSuper1}"" />
      <SuperSkill2 ID1=""{modData.CusSuper2}"" />
      <SuperSkill3 ID1=""{modData.CusSuper3}"" />
      <SuperSkill4 ID1=""{modData.CusSuper4}"" />
      <UltimateSkill1 ID1=""{modData.CusUltimate1}"" />
      <UltimateSkill2 ID1=""{modData.CusUltimate2}"" />
      <EvasiveSkill ID1=""{modData.CusEvasive}"" />
      <BlastType ID1=""65535"" />
      <AwokenSkill ID1=""0"" />
    </Skillset>
  </Skillsets>";
            
            text = text.Replace("  </Skillsets>", skillsetEntry);
            File.WriteAllText(cusPath, text);
            
            RunXMLSerializer("custom_skill.cus.xml");
        }

        private void UpdateAUR(ModData modData, int charId)
        {
            RunXMLSerializer("aura_setting.aur");
            
            var aurPath = Settings.Default.datafolder + @"\system\aura_setting.aur.xml";
            var text = File.ReadAllText(aurPath);
            
            var glare = modData.AurGlare == 1 ? "True" : "False";
            var auraEntry = $"    <CharacterAura Chara_ID=\"{charId}\" Costume=\"0\" Aura_ID=\"{modData.AurId}\" Glare=\"{glare}\" />\r\n  </CharacterAuras>";
            
            text = text.Replace("  </CharacterAuras>", auraEntry);
            File.WriteAllText(aurPath, text);
            
            RunXMLSerializer("aura_setting.aur.xml");
        }

        private void UpdatePSC(ModData modData, int charId)
        {
            RunXMLSerializer("parameter_spec_char.psc");
            
            var pscPath = Settings.Default.datafolder + @"\system\parameter_spec_char.psc.xml";
            var text = File.ReadAllText(pscPath);
            
            var pscEntry = CreatePSCEntry(modData, charId);
            text = text.Replace("  </Configuration>\r\n</PSC>", pscEntry);
            File.WriteAllText(pscPath, text);
            
            RunXMLSerializer("parameter_spec_char.psc.xml");
        }

        private string CreatePSCEntry(ModData modData, int charId)
        {
            return $@"    <PSC_Entry Chara_ID=""{charId}"">
      <PscSpecEntry Costume=""{modData.PscCostume}"" Preset=""{modData.PscPreset}"">
        <Camera_Position value=""{modData.PscCameraPos}"" />
        <I_12 value=""{modData.PscI12}"" />
        <Health value=""{modData.PscHealth}"" />
        <F_20 value=""{modData.PscF20}"" />
        <Ki value=""{modData.PscKi}"" />
        <Ki_Recharge value=""{modData.PscKiRecharge}"" />
        <I_32 value=""{modData.PscI32}"" />
        <I_36 value=""{modData.PscI36}"" />
        <I_40 value=""{modData.PscI40}"" />
        <Stamina value=""{modData.PscStamina}"" />
        <Stamina_Recharge value=""{modData.PscStaminaRecharge}"" />
        <F_52 value=""{modData.PscF52}"" />
        <F_56 value=""{modData.PscF56}"" />
        <I_60 value=""{modData.PscI60}"" />
        <Basic_Atk_Defense value=""{modData.PscBasicAtkDef}"" />
        <Basic_Ki_Defense value=""{modData.PscBasicKiDef}"" />
        <Strike_Atk_Defense value=""{modData.PscStrikeAtkDef}"" />
        <Super_Ki_Defense value=""{modData.PscSuperKiDef}"" />
        <Ground_Speed value=""{modData.PscGroundSpeed}"" />
        <Air_Speed value=""{modData.PscAirSpeed}"" />
        <Boost_Speed value=""{modData.PscBoostSpeed}"" />
        <Dash_Speed value=""{modData.PscDashSpeed}"" />
        <F_96 value=""{modData.PscF96}"" />
        <Reinforcement_Skill_Duration value=""{modData.PscReinforcementSkill}"" />
        <F_104 value=""{modData.PscF104}"" />
        <Revival_HP_Amount value=""{modData.PscRevivalHpAmount}"" />
        <Reviving_Speed value=""{modData.PscRevivalSpeed}"" />
        <F_116 value=""{modData.PscF116}"" />
        <F_120 value=""{modData.PscF120}"" />
        <F_124 value=""{modData.PscF124}"" />
        <F_128 value=""{modData.PscF128}"" />
        <F_132 value=""{modData.PscF132}"" />
        <F_136 value=""{modData.PscF136}"" />
        <I_140 value=""{modData.PscI140}"" />
        <F_144 value=""{modData.PscF144}"" />
        <F_148 value=""{modData.PscF148}"" />
        <F_152 value=""{modData.PscF152}"" />
        <F_156 value=""{modData.PscF156}"" />
        <F_160 value=""{modData.PscF160}"" />
        <F_164 value=""{modData.PscF164}"" />
        <Z-Soul value=""{modData.PscZSoul}"" />
        <I_172 value=""{modData.PscI172}"" />
        <I_176 value=""{modData.PscI176}"" />
        <F_180 value=""{modData.PscF180}"" />
      </PscSpecEntry>
    </PSC_Entry>
  </Configuration>
</PSC>";
        }

        private void UpdateCharacterList(ModData modData, int charId)
        {
            var charaList = Settings.Default.datafolder + @"\XVP_SLOTS.xs";
            var text = new StringBuilder();

            foreach (string line in File.ReadAllLines(charaList))
            {
                text.Append(line.Replace("{[JCO,0,0,0,110,111]}", 
                    "{[JCO,0,0,0,110,111]}{[" + modData.CmsBcs + $",0,0,0,{modData.Vox1},{modData.Vox2}]}}"));
            }

            using (var file = new StreamWriter(File.Create(charaList)))
            {
                file.Write(text.ToString());
            }
        }

        private void UpdateCharacterMessages(ModData modData, int charId, string language)
        {
            var msgPath = Settings.Default.datafolder + @"/msg/proper_noun_character_name_" + language + ".msg";
            if (!File.Exists(msgPath)) return;

            var msgFile = msgStream.Load(msgPath);
            
            // Add null and bounds checking
            if (msgFile?.data == null || msgFile.data.Length == 0)
            {
                MessageBox.Show("Error: Invalid message file structure", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var expand = new msgData[msgFile.data.Length + 1];
            Array.Copy(msgFile.data, expand, msgFile.data.Length);

            // Safe access to last element
            var nameId = msgFile.data[msgFile.data.Length - 1].NameID;
            if (string.IsNullOrEmpty(nameId) || nameId.Length < 3)
            {
                MessageBox.Show("Error: Invalid name ID format in message file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var endId = int.Parse(nameId.Substring(nameId.Length - 3, 3));

            expand[expand.Length - 1].ID = msgFile.data.Length;
            expand[expand.Length - 1].Lines = new string[] { modData.MsgCharacterName };
            expand[expand.Length - 1].NameID = "chara_" + modData.CmsBcs + "_" + endId.ToString("000");

            msgFile.data = expand;
            msgStream.Save(msgFile, msgPath);
        }

        private void UpdateCharacterTextures()
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
                    sw.WriteLine("cd " + Settings.Default.datafolder + @"\ui\texture");
                    sw.WriteLine(@"embpack.exe CHARA01");
                }
            }
        }

        private void UpdateSkillCUS(ModData modData)
        {
            RunXMLSerializer("custom_skill.cus");
            
            var cusPath = Settings.Default.datafolder + @"\system\custom_skill.cus.xml";
            var text = File.ReadAllText(cusPath);
            
            var skillEntry = CreateSkillEntry(modData);
            
            switch (modData.SkillType)
            {
                case "SPA":
                    text = text.Replace("\r\n  </SuperSkills>", skillEntry + "</SuperSkills>");
                    break;
                case "ULT":
                    text = text.Replace("\r\n  </UltimateSkills>", skillEntry + "</UltimateSkills>");
                    break;
                case "ESC":
                    text = text.Replace("\r\n  </EvasiveSkills>", skillEntry + "</EvasiveSkills>");
                    break;
            }
            
            File.WriteAllText(cusPath, text);
            RunXMLSerializer("custom_skill.cus.xml");
        }

        private string CreateSkillEntry(ModData modData)
        {
            return $@"<Skill ShortName=""{modData.SkillShortName}"" ID1=""{modData.SkillId1}"" ID2=""{modData.SkillId2}"">
<I_04 value=""{modData.SkillI04}"" />
<Race_Lock value=""{modData.SkillRaceLock}"" />
<Type value=""{modData.SkillType}"" />
<FilesLoaded Flags=""{modData.SkillFilesLoaded}"" />
<PartSet value=""{modData.SkillPartSet}"" />
<I_18 value=""{modData.SkillI18}"" />
<EAN Path=""{modData.SkillEan}"" />
<CAM_EAN Path=""{modData.SkillCamEan}"" />
<EEPK Path=""{modData.SkillEepk}"" />
<ACB_SE Path=""{modData.SkillAcbSe}"" />
<ACB_VOX Path=""{modData.SkillAcbVox}"" />
<AFTER_BAC Path=""{modData.SkillAfterBac}"" />
<AFTER_BCM Path=""{modData.SkillAfterBcm}"" />
<I_48 value=""{modData.SkillI48}"" />
<I_50 value=""{modData.SkillI50}"" />
<I_52 value=""{modData.SkillI52}"" />
<I_54 value=""{modData.SkillI54}"" />
<PUP ID=""{modData.SkillPup}"" />
<CUS_Aura value=""{modData.SkillCusAura}"" />
<TransformCharaSwap Chara_ID=""{modData.SkillTransformCharaSwap}"" />
<Skillset_Change ModelPreset=""{modData.SkillSkillsetChange}"" />
<Num_Of_Transforms value=""{modData.SkillNumOfTransforms}"" />
<I_66 value=""{modData.SkillI66}"" />
</Skill>";
        }

        private void UpdateSkillMessages(ModData modData, string language)
        {
            var skillTypeMsg = GetSkillTypeMessage(modData.SkillType);
            var msgFileName = Settings.Default.datafolder + @"/msg/proper_noun_skill_" + modData.SkillType.ToLower() + "_name_" + language + ".msg";
            var msgFileInfo = Settings.Default.datafolder + @"/msg/proper_noun_skill_" + modData.SkillType.ToLower() + "_info_" + language + ".msg";
            
            UpdateMessageFile(msgFileName, modData.MsgSkillName, skillTypeMsg + modData.SkillId1.PadLeft(3, '0'));
            UpdateMessageFile(msgFileInfo, modData.MsgSkillDesc, skillTypeMsg + modData.SkillId1.PadLeft(3, '0'));
        }

        private string GetSkillTypeMessage(string skillType)
        {
            switch (skillType)
            {
                case "SPA": return "spe_skill_";
                case "ULT": return "ult_";
                case "ESC": return "avoid_skill_";
                default: return "";
            }
        }

        private void UpdateMessageFile(string filePath, string message, string nameId)
        {
            if (!File.Exists(filePath)) return;

            var msgFile = msgStream.Load(filePath);
            
            // Add null and bounds checking
            if (msgFile?.data == null || msgFile.data.Length == 0)
            {
                MessageBox.Show("Error: Invalid message file structure", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var expand = new msgData[msgFile.data.Length + 1];
            Array.Copy(msgFile.data, expand, msgFile.data.Length);
            
            // Safe access to last element
            var lastNameId = msgFile.data[msgFile.data.Length - 1].NameID;
            if (string.IsNullOrEmpty(lastNameId) || lastNameId.Length < 3)
            {
                MessageBox.Show("Error: Invalid name ID format in message file", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var endId = int.Parse(lastNameId.Substring(lastNameId.Length - 3, 3));
            
            expand[expand.Length - 1].ID = msgFile.data.Length;
            expand[expand.Length - 1].Lines = new string[] { message };
            expand[expand.Length - 1].NameID = nameId;
            
            msgFile.data = expand;
            msgStream.Save(msgFile, filePath);
        }

        private void RunXMLSerializer(string fileName)
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
                    sw.WriteLine("cd " + Settings.Default.datafolder + @"\system");
                    sw.WriteLine($@"XMLSerializer.exe {fileName}");
                }
            }

            process.WaitForExit();
        }

        private void AddModToListView(ModData modData, string modType, ListView lvMods)
        {
            var row = new string[] { modData.ModName, modData.ModAuthor, modType };
            var lvi = new ListViewItem(row);
            lvMods.Items.Add(lvi);
            
            // Save the updated list
            SaveModList(lvMods);
        }

        private void SaveModList(ListView lvMods)
        {
            Properties.Settings.Default.modlist = new StringCollection();
            Properties.Settings.Default.modlist.AddRange(
                (from i in lvMods.Items.Cast<ListViewItem>()
                 select string.Join("|", 
                     from si in i.SubItems.Cast<ListViewItem.ListViewSubItem>()
                     select si.Text)).ToArray());
            Properties.Settings.Default.Save();
        }
    }
} 