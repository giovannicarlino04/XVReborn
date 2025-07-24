namespace XVReborn
{
    public class ModData
    {
        // Basic mod information
        public string ModType { get; set; } = "";
        public string ModName { get; set; } = "";
        public string ModAuthor { get; set; } = "";
        public string ModVersion { get; set; } = "";

        // Aura settings
        public int AurId { get; set; } = 0;
        public int AurGlare { get; set; } = 0;

        // Character model settings
        public string CmsBcs { get; set; } = "";
        public string CmsEan { get; set; } = "";
        public string CmsFceEan { get; set; } = "";
        public string CmsCamEan { get; set; } = "";
        public string CmsBac { get; set; } = "";
        public string CmsBcm { get; set; } = "";
        public string CmsBai { get; set; } = "";

        // Character sound settings
        public string Cso1 { get; set; } = "";
        public string Cso2 { get; set; } = "";
        public string Cso3 { get; set; } = "";
        public string Cso4 { get; set; } = "";

        // Custom skill settings
        public string CusSuper1 { get; set; } = "";
        public string CusSuper2 { get; set; } = "";
        public string CusSuper3 { get; set; } = "";
        public string CusSuper4 { get; set; } = "";
        public string CusUltimate1 { get; set; } = "";
        public string CusUltimate2 { get; set; } = "";
        public string CusEvasive { get; set; } = "";

        // Parameter spec character settings
        public string PscCostume { get; set; } = "";
        public string PscPreset { get; set; } = "";
        public string PscCameraPos { get; set; } = "";
        public string PscHealth { get; set; } = "";
        public string PscI12 { get; set; } = "";
        public string PscF20 { get; set; } = "";
        public string PscKi { get; set; } = "";
        public string PscKiRecharge { get; set; } = "";
        public string PscI32 { get; set; } = "";
        public string PscI36 { get; set; } = "";
        public string PscI40 { get; set; } = "";
        public string PscStamina { get; set; } = "";
        public string PscStaminaRecharge { get; set; } = "";
        public string PscF52 { get; set; } = "";
        public string PscF56 { get; set; } = "";
        public string PscI60 { get; set; } = "";
        public string PscBasicAtkDef { get; set; } = "";
        public string PscBasicKiDef { get; set; } = "";
        public string PscStrikeAtkDef { get; set; } = "";
        public string PscSuperKiDef { get; set; } = "";
        public string PscGroundSpeed { get; set; } = "";
        public string PscAirSpeed { get; set; } = "";
        public string PscBoostSpeed { get; set; } = "";
        public string PscDashSpeed { get; set; } = "";
        public string PscF96 { get; set; } = "";
        public string PscReinforcementSkill { get; set; } = "";
        public string PscF104 { get; set; } = "";
        public string PscRevivalHpAmount { get; set; } = "";
        public string PscRevivalSpeed { get; set; } = "";
        public string PscF116 { get; set; } = "";
        public string PscF120 { get; set; } = "";
        public string PscF124 { get; set; } = "";
        public string PscF128 { get; set; } = "";
        public string PscF132 { get; set; } = "";
        public string PscF136 { get; set; } = "";
        public string PscI140 { get; set; } = "";
        public string PscF144 { get; set; } = "";
        public string PscF148 { get; set; } = "";
        public string PscF152 { get; set; } = "";
        public string PscF156 { get; set; } = "";
        public string PscF160 { get; set; } = "";
        public string PscF164 { get; set; } = "";
        public string PscZSoul { get; set; } = "";
        public string PscI172 { get; set; } = "";
        public string PscI176 { get; set; } = "";
        public string PscF180 { get; set; } = "";

        // Message settings
        public string MsgCharacterName { get; set; } = "";
        public string MsgCostumeName { get; set; } = "";
        public string MsgSkillName { get; set; } = "";
        public string MsgSkillDesc { get; set; } = "";

        // Voice settings
        public short Vox1 { get; set; } = -1;
        public short Vox2 { get; set; } = -1;

        // Skill settings
        public string SkillType { get; set; } = "";
        public string SkillShortName { get; set; } = "";
        public string SkillId1 { get; set; } = "";
        public string SkillId2 { get; set; } = "";
        public string SkillI04 { get; set; } = "";
        public string SkillRaceLock { get; set; } = "";
        public string SkillFilesLoaded { get; set; } = "";
        public string SkillPartSet { get; set; } = "";
        public string SkillI18 { get; set; } = "";
        public string SkillEan { get; set; } = "";
        public string SkillCamEan { get; set; } = "";
        public string SkillEepk { get; set; } = "";
        public string SkillAcbSe { get; set; } = "";
        public string SkillAcbVox { get; set; } = "";
        public string SkillAfterBac { get; set; } = "";
        public string SkillAfterBcm { get; set; } = "";
        public string SkillI48 { get; set; } = "";
        public string SkillI50 { get; set; } = "";
        public string SkillI52 { get; set; } = "";
        public string SkillI54 { get; set; } = "";
        public string SkillPup { get; set; } = "";
        public string SkillCusAura { get; set; } = "";
        public string SkillTransformCharaSwap { get; set; } = "";
        public string SkillSkillsetChange { get; set; } = "";
        public string SkillNumOfTransforms { get; set; } = "";
        public string SkillI66 { get; set; } = "";
    }
} 