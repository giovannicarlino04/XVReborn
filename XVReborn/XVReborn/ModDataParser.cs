using System;
using System.IO;
using System.Xml;

namespace XVReborn
{
    public class ModDataParser
    {
        public ModData ParseModXml(string xmlFilePath)
        {
            var modData = new ModData();
            var xmlData = File.ReadAllText(xmlFilePath);

            using (var reader = XmlReader.Create(new StringReader(xmlData)))
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        ParseElement(reader, modData);
                    }
                }
            }

            return modData;
        }

        private void ParseElement(XmlReader reader, ModData modData)
        {
            switch (reader.Name)
            {
                case "XVMOD":
                    modData.ModType = reader.GetAttribute("type") ?? "";
                    break;
                case "MOD_NAME":
                    modData.ModName = GetAttributeValue(reader);
                    break;
                case "MOD_AUTHOR":
                    modData.ModAuthor = GetAttributeValue(reader);
                    break;
                case "MOD_VERSION":
                    modData.ModVersion = GetAttributeValue(reader);
                    break;
                case "AUR_ID":
                    modData.AurId = ParseIntAttribute(reader);
                    break;
                case "AUR_GLARE":
                    modData.AurGlare = ParseIntAttribute(reader);
                    break;
                case "CMS_BCS":
                    modData.CmsBcs = GetAttributeValue(reader);
                    break;
                case "CMS_EAN":
                    modData.CmsEan = GetAttributeValue(reader);
                    break;
                case "CMS_FCE_EAN":
                    modData.CmsFceEan = GetAttributeValue(reader);
                    break;
                case "CMS_CAM_EAN":
                    modData.CmsCamEan = GetAttributeValue(reader);
                    break;
                case "CMS_BAC":
                    modData.CmsBac = GetAttributeValue(reader);
                    break;
                case "CMS_BCM":
                    modData.CmsBcm = GetAttributeValue(reader);
                    break;
                case "CMS_BAI":
                    modData.CmsBai = GetAttributeValue(reader);
                    break;
                case "CSO_1":
                    modData.Cso1 = GetAttributeValue(reader);
                    break;
                case "CSO_2":
                    modData.Cso2 = GetAttributeValue(reader);
                    break;
                case "CSO_3":
                    modData.Cso3 = GetAttributeValue(reader);
                    break;
                case "CSO_4":
                    modData.Cso4 = GetAttributeValue(reader);
                    break;
                case "CUS_SUPER_1":
                    modData.CusSuper1 = GetAttributeValue(reader);
                    break;
                case "CUS_SUPER_2":
                    modData.CusSuper2 = GetAttributeValue(reader);
                    break;
                case "CUS_SUPER_3":
                    modData.CusSuper3 = GetAttributeValue(reader);
                    break;
                case "CUS_SUPER_4":
                    modData.CusSuper4 = GetAttributeValue(reader);
                    break;
                case "CUS_ULTIMATE_1":
                    modData.CusUltimate1 = GetAttributeValue(reader);
                    break;
                case "CUS_ULTIMATE_2":
                    modData.CusUltimate2 = GetAttributeValue(reader);
                    break;
                case "CUS_EVASIVE":
                    modData.CusEvasive = GetAttributeValue(reader);
                    break;
                case "PSC_COSTUME":
                    modData.PscCostume = GetAttributeValue(reader);
                    break;
                case "PSC_PRESET":
                    modData.PscPreset = GetAttributeValue(reader);
                    break;
                case "PSC_CAMERA_POS":
                    modData.PscCameraPos = GetAttributeValue(reader);
                    break;
                case "PSC_HEALTH":
                    modData.PscHealth = GetAttributeValue(reader);
                    break;
                case "PSC_I_12":
                    modData.PscI12 = GetAttributeValue(reader);
                    break;
                case "PSC_F_20":
                    modData.PscF20 = GetAttributeValue(reader);
                    break;
                case "PSC_KI":
                    modData.PscKi = GetAttributeValue(reader);
                    break;
                case "PSC_KI_RECHARGE":
                    modData.PscKiRecharge = GetAttributeValue(reader);
                    break;
                case "PSC_I_32":
                    modData.PscI32 = GetAttributeValue(reader);
                    break;
                case "PSC_I_36":
                    modData.PscI36 = GetAttributeValue(reader);
                    break;
                case "PSC_I_40":
                    modData.PscI40 = GetAttributeValue(reader);
                    break;
                case "PSC_STAMINA":
                    modData.PscStamina = GetAttributeValue(reader);
                    break;
                case "PSC_STAMINA_RECHARGE":
                    modData.PscStaminaRecharge = GetAttributeValue(reader);
                    break;
                case "PSC_F_52":
                    modData.PscF52 = GetAttributeValue(reader);
                    break;
                case "PSC_F_56":
                    modData.PscF56 = GetAttributeValue(reader);
                    break;
                case "PSC_I_60":
                    modData.PscI60 = GetAttributeValue(reader);
                    break;
                case "PSC_BASIC_ATK_DEF":
                    modData.PscBasicAtkDef = GetAttributeValue(reader);
                    break;
                case "PSC_BASIC_KI_DEF":
                    modData.PscBasicKiDef = GetAttributeValue(reader);
                    break;
                case "PSC_STRIKE_ATK_DEF":
                    modData.PscStrikeAtkDef = GetAttributeValue(reader);
                    break;
                case "PSC_SUPER_KI_DEF":
                    modData.PscSuperKiDef = GetAttributeValue(reader);
                    break;
                case "PSC_GROUND_SPEED":
                    modData.PscGroundSpeed = GetAttributeValue(reader);
                    break;
                case "PSC_AIR_SPEED":
                    modData.PscAirSpeed = GetAttributeValue(reader);
                    break;
                case "PSC_BOOST_SPEED":
                    modData.PscBoostSpeed = GetAttributeValue(reader);
                    break;
                case "PSC_DASH_SPEED":
                    modData.PscDashSpeed = GetAttributeValue(reader);
                    break;
                case "PSC_F_96":
                    modData.PscF96 = GetAttributeValue(reader);
                    break;
                case "PSC_REINFORCEMENT_SKILL":
                    modData.PscReinforcementSkill = GetAttributeValue(reader);
                    break;
                case "PSC_F_104":
                    modData.PscF104 = GetAttributeValue(reader);
                    break;
                case "PSC_REVIVAL_HP_AMOUNT":
                    modData.PscRevivalHpAmount = GetAttributeValue(reader);
                    break;
                case "PSC_REVIVAL_SPEED":
                    modData.PscRevivalSpeed = GetAttributeValue(reader);
                    break;
                case "PSC_F_116":
                    modData.PscF116 = GetAttributeValue(reader);
                    break;
                case "PSC_F_120":
                    modData.PscF120 = GetAttributeValue(reader);
                    break;
                case "PSC_F_124":
                    modData.PscF124 = GetAttributeValue(reader);
                    break;
                case "PSC_F_128":
                    modData.PscF128 = GetAttributeValue(reader);
                    break;
                case "PSC_F_132":
                    modData.PscF132 = GetAttributeValue(reader);
                    break;
                case "PSC_F_136":
                    modData.PscF136 = GetAttributeValue(reader);
                    break;
                case "PSC_I_140":
                    modData.PscI140 = GetAttributeValue(reader);
                    break;
                case "PSC_F_144":
                    modData.PscF144 = GetAttributeValue(reader);
                    break;
                case "PSC_F_148":
                    modData.PscF148 = GetAttributeValue(reader);
                    break;
                case "PSC_F_152":
                    modData.PscF152 = GetAttributeValue(reader);
                    break;
                case "PSC_F_156":
                    modData.PscF156 = GetAttributeValue(reader);
                    break;
                case "PSC_F_160":
                    modData.PscF160 = GetAttributeValue(reader);
                    break;
                case "PSC_F_164":
                    modData.PscF164 = GetAttributeValue(reader);
                    break;
                case "PSC_Z_SOUL":
                    modData.PscZSoul = GetAttributeValue(reader);
                    break;
                case "PSC_I_172":
                    modData.PscI172 = GetAttributeValue(reader);
                    break;
                case "PSC_I_176":
                    modData.PscI176 = GetAttributeValue(reader);
                    break;
                case "PSC_F_180":
                    modData.PscF180 = GetAttributeValue(reader);
                    break;
                case "MSG_CHARACTER_NAME":
                    modData.MsgCharacterName = GetAttributeValue(reader);
                    break;
                case "MSG_COSTUME_NAME":
                    modData.MsgCostumeName = GetAttributeValue(reader);
                    break;
                case "MSG_SKILL_NAME":
                    modData.MsgSkillName = GetAttributeValue(reader);
                    break;
                case "MSG_SKILL_DESC":
                    modData.MsgSkillDesc = GetAttributeValue(reader);
                    break;
                case "VOX_1":
                    modData.Vox1 = ParseShortAttribute(reader);
                    break;
                case "VOX_2":
                    modData.Vox2 = ParseShortAttribute(reader);
                    break;
                case "SKILL_TYPE":
                    modData.SkillType = GetAttributeValue(reader);
                    break;
                case "ShortName":
                    modData.SkillShortName = GetAttributeValue(reader);
                    break;
                case "ID1":
                    modData.SkillId1 = GetAttributeValue(reader);
                    break;
                case "ID2":
                    modData.SkillId2 = GetAttributeValue(reader);
                    break;
                case "I_04":
                    modData.SkillI04 = GetAttributeValue(reader);
                    break;
                case "Race_Lock":
                    modData.SkillRaceLock = GetAttributeValue(reader);
                    break;
                case "Type":
                    modData.SkillType = GetAttributeValue(reader);
                    break;
                case "FilesLoaded":
                    modData.SkillFilesLoaded = GetAttributeValue(reader);
                    break;
                case "PartSet":
                    modData.SkillPartSet = GetAttributeValue(reader);
                    break;
                case "I_18":
                    modData.SkillI18 = GetAttributeValue(reader);
                    break;
                case "EAN":
                    modData.SkillEan = GetAttributeValue(reader);
                    break;
                case "CAM_EAN":
                    modData.SkillCamEan = GetAttributeValue(reader);
                    break;
                case "EEPK":
                    modData.SkillEepk = GetAttributeValue(reader);
                    break;
                case "ACB_SE":
                    modData.SkillAcbSe = GetAttributeValue(reader);
                    break;
                case "ACB_VOX":
                    modData.SkillAcbVox = GetAttributeValue(reader);
                    break;
                case "AFTER_BAC":
                    modData.SkillAfterBac = GetAttributeValue(reader);
                    break;
                case "AFTER_BCM":
                    modData.SkillAfterBcm = GetAttributeValue(reader);
                    break;
                case "I_48":
                    modData.SkillI48 = GetAttributeValue(reader);
                    break;
                case "I_50":
                    modData.SkillI50 = GetAttributeValue(reader);
                    break;
                case "I_52":
                    modData.SkillI52 = GetAttributeValue(reader);
                    break;
                case "I_54":
                    modData.SkillI54 = GetAttributeValue(reader);
                    break;
                case "PUP":
                    modData.SkillPup = GetAttributeValue(reader);
                    break;
                case "CUS_Aura":
                    modData.SkillCusAura = GetAttributeValue(reader);
                    break;
                case "TransformCharaSwap":
                    modData.SkillTransformCharaSwap = GetAttributeValue(reader);
                    break;
                case "Skillset_Change":
                    modData.SkillSkillsetChange = GetAttributeValue(reader);
                    break;
                case "Num_Of_Transforms":
                    modData.SkillNumOfTransforms = GetAttributeValue(reader);
                    break;
                case "I_66":
                    modData.SkillI66 = GetAttributeValue(reader);
                    break;
            }
        }

        private string GetAttributeValue(XmlReader reader)
        {
            return reader.HasAttributes ? reader.GetAttribute("value")?.Trim() ?? "" : "";
        }

        private int ParseIntAttribute(XmlReader reader)
        {
            if (!reader.HasAttributes) return 0;
            
            var value = reader.GetAttribute("value")?.Trim();
            if (int.TryParse(value, out int result))
                return result;
            
            return 0;
        }

        private short ParseShortAttribute(XmlReader reader)
        {
            if (!reader.HasAttributes) return -1;
            
            var value = reader.GetAttribute("value")?.Trim();
            if (short.TryParse(value, out short result))
                return result;
            
            return -1;
        }
    }
} 