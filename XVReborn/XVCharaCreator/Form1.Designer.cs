using System;
using System.Drawing;
using System.Windows.Forms;

namespace XVCharaCreator
{
    partial class Form1
    {

        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            
            // Initialize all controls
            menuStrip1 = new MenuStrip();
            fileToolStripMenuItem = new ToolStripMenuItem();
            openToolStripMenuItem = new ToolStripMenuItem();
            buildXVModFileToolStripMenuItem = new ToolStripMenuItem();
            toolStripSeparator1 = new ToolStripSeparator();
            sToolStripMenuItem = new ToolStripMenuItem();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            button2 = new Button();
            button1 = new Button();
            textBox2 = new TextBox();
            label30 = new Label();
            textBox1 = new TextBox();
            label27 = new Label();
            btnGenID = new Button();
            txtCharID = new TextBox();
            btnFolder = new Button();
            txtName = new TextBox();
            txtAuthor = new TextBox();
            txtFolder = new TextBox();
            label3 = new Label();
            label2 = new Label();
            label5 = new Label();
            label1 = new Label();
            tabPage2 = new TabPage();
            txtAuraID = new TextBox();
            cbAuraGlare = new CheckBox();
            label6 = new Label();
            tabPage3 = new TabPage();
            txtBAI = new TextBox();
            label13 = new Label();
            txtBCM = new TextBox();
            label12 = new Label();
            txtBAC = new TextBox();
            label11 = new Label();
            txtCAMEAN = new TextBox();
            label10 = new Label();
            txtFCEEAN = new TextBox();
            label9 = new Label();
            txtEAN = new TextBox();
            label8 = new Label();
            txtBCS = new TextBox();
            label7 = new Label();
            tabPage4 = new TabPage();
            txtCSO4 = new TextBox();
            label14 = new Label();
            txtCSO3 = new TextBox();
            label15 = new Label();
            txtCSO2 = new TextBox();
            label16 = new Label();
            txtCSO1 = new TextBox();
            label17 = new Label();
            tabPage5 = new TabPage();
            label23 = new Label();
            label22 = new Label();
            label21 = new Label();
            label24 = new Label();
            label20 = new Label();
            label19 = new Label();
            label18 = new Label();
            cbEvasive = new ComboBox();
            cbUltimate2 = new ComboBox();
            cbSuper4 = new ComboBox();
            cbUltimate1 = new ComboBox();
            cbSuper3 = new ComboBox();
            cbSuper2 = new ComboBox();
            cbSuper1 = new ComboBox();
            tabPage6 = new TabPage();
            panelPSC = new Panel();
            tabPage7 = new TabPage();
            label26 = new Label();
            label25 = new Label();
            txtMSG2 = new TextBox();
            txtMSG1 = new TextBox();
            tabPage8 = new TabPage();
            txtVOX2 = new TextBox();
            txtVOX1 = new TextBox();
            label29 = new Label();
            label28 = new Label();
            
            // Initialize PSC controls
            txtCostume = new TextBox();
            txtPreset = new TextBox();
            txtCameraPosition = new TextBox();
            txtHealth = new TextBox();
            txtI12 = new TextBox();
            txtF20 = new TextBox();
            txtKi = new TextBox();
            txtKiRecharge = new TextBox();
            txtI32 = new TextBox();
            txtI36 = new TextBox();
            txtI40 = new TextBox();
            txtStamina = new TextBox();
            txtStaminaRecharge = new TextBox();
            txtF52 = new TextBox();
            txtF56 = new TextBox();
            txtI60 = new TextBox();
            txtBasicAtkDef = new TextBox();
            txtBasicKiDef = new TextBox();
            txtStrikeAtkDef = new TextBox();
            txtSuperKiDef = new TextBox();
            txtGroundSpeed = new TextBox();
            txtAirSpeed = new TextBox();
            txtBoostSpeed = new TextBox();
            txtDashSpeed = new TextBox();
            txtF96 = new TextBox();
            txtReinforcementSkill = new TextBox();
            txtF104 = new TextBox();
            txtRevivalHpAmount = new TextBox();
            txtRevivingSpeed = new TextBox();
            txtF116 = new TextBox();
            txtF120 = new TextBox();
            txtF124 = new TextBox();
            txtF128 = new TextBox();
            txtF132 = new TextBox();
            txtF136 = new TextBox();
            txtI140 = new TextBox();
            txtF144 = new TextBox();
            txtF148 = new TextBox();
            txtF152 = new TextBox();
            txtF156 = new TextBox();
            txtF160 = new TextBox();
            txtF164 = new TextBox();
            txtZSoul = new TextBox();
            txtI172 = new TextBox();
            txtI176 = new TextBox();
            txtF180 = new TextBox();
            
            // Initialize PSC labels
            label4 = new Label();
            label31 = new Label();
            label32 = new Label();
            label33 = new Label();
            label34 = new Label();
            label35 = new Label();
            label36 = new Label();
            label37 = new Label();
            label38 = new Label();
            label39 = new Label();
            label40 = new Label();
            label41 = new Label();
            label42 = new Label();
            label43 = new Label();
            label44 = new Label();
            label45 = new Label();
            label46 = new Label();
            menuStrip1.SuspendLayout();
            tabControl1.SuspendLayout();
            tabPage1.SuspendLayout();
            tabPage2.SuspendLayout();
            tabPage3.SuspendLayout();
            tabPage4.SuspendLayout();
            tabPage5.SuspendLayout();
            tabPage7.SuspendLayout();
            tabPage8.SuspendLayout();
            tabPage6.SuspendLayout();
            SuspendLayout();
            // 
            // txtCostume
            // 
            txtCostume.Location = new Point(120, 20);
            txtCostume.Name = "txtCostume";
            txtCostume.Size = new Size(100, 23);
            txtCostume.TabIndex = 0;
            // 
            // txtPreset
            // 
            txtPreset.Location = new Point(120, 50);
            txtPreset.Name = "txtPreset";
            txtPreset.Size = new Size(100, 23);
            txtPreset.TabIndex = 1;
            // 
            // txtCameraPosition
            // 
            txtCameraPosition.Location = new Point(120, 80);
            txtCameraPosition.Name = "txtCameraPosition";
            txtCameraPosition.Size = new Size(100, 23);
            txtCameraPosition.TabIndex = 2;
            // 
            // txtHealth
            // 
            txtHealth.Location = new Point(120, 110);
            txtHealth.Name = "txtHealth";
            txtHealth.Size = new Size(100, 23);
            txtHealth.TabIndex = 3;
            // 
            // txtI12
            // 
            txtI12.Location = new Point(120, 140);
            txtI12.Name = "txtI12";
            txtI12.Size = new Size(100, 23);
            txtI12.TabIndex = 4;
            // 
            // txtF20
            // 
            txtF20.Location = new Point(120, 170);
            txtF20.Name = "txtF20";
            txtF20.Size = new Size(100, 23);
            txtF20.TabIndex = 5;
            // 
            // txtKi
            // 
            txtKi.Location = new Point(120, 200);
            txtKi.Name = "txtKi";
            txtKi.Size = new Size(100, 23);
            txtKi.TabIndex = 6;
            // 
            // txtKiRecharge
            // 
            txtKiRecharge.Location = new Point(120, 230);
            txtKiRecharge.Name = "txtKiRecharge";
            txtKiRecharge.Size = new Size(100, 23);
            txtKiRecharge.TabIndex = 7;
            // 
            // txtI32
            // 
            txtI32.Location = new Point(120, 260);
            txtI32.Name = "txtI32";
            txtI32.Size = new Size(100, 23);
            txtI32.TabIndex = 8;
            // 
            // txtI36
            // 
            txtI36.Location = new Point(120, 290);
            txtI36.Name = "txtI36";
            txtI36.Size = new Size(100, 23);
            txtI36.TabIndex = 9;
            // 
            // txtI40
            // 
            txtI40.Location = new Point(120, 320);
            txtI40.Name = "txtI40";
            txtI40.Size = new Size(100, 23);
            txtI40.TabIndex = 10;
            // 
            // txtStamina
            // 
            txtStamina.Location = new Point(120, 350);
            txtStamina.Name = "txtStamina";
            txtStamina.Size = new Size(100, 23);
            txtStamina.TabIndex = 11;
            // 
            // txtStaminaRecharge
            // 
            txtStaminaRecharge.Location = new Point(120, 380);
            txtStaminaRecharge.Name = "txtStaminaRecharge";
            txtStaminaRecharge.Size = new Size(100, 23);
            txtStaminaRecharge.TabIndex = 12;
            // 
            // txtF52
            // 
            txtF52.Location = new Point(120, 410);
            txtF52.Name = "txtF52";
            txtF52.Size = new Size(100, 23);
            txtF52.TabIndex = 13;
            // 
            // txtF56
            // 
            txtF56.Location = new Point(120, 440);
            txtF56.Name = "txtF56";
            txtF56.Size = new Size(100, 23);
            txtF56.TabIndex = 14;
            // 
            // txtI60
            // 
            txtI60.Location = new Point(120, 470);
            txtI60.Name = "txtI60";
            txtI60.Size = new Size(100, 23);
            txtI60.TabIndex = 15;
            // 
            // txtBasicAtkDef
            // 
            txtBasicAtkDef.Location = new Point(120, 500);
            txtBasicAtkDef.Name = "txtBasicAtkDef";
            txtBasicAtkDef.Size = new Size(100, 23);
            txtBasicAtkDef.TabIndex = 16;
            // 
            // txtBasicKiDef
            // 
            txtBasicKiDef.Location = new Point(120, 530);
            txtBasicKiDef.Name = "txtBasicKiDef";
            txtBasicKiDef.Size = new Size(100, 23);
            txtBasicKiDef.TabIndex = 17;
            // 
            // txtStrikeAtkDef
            // 
            txtStrikeAtkDef.Location = new Point(120, 560);
            txtStrikeAtkDef.Name = "txtStrikeAtkDef";
            txtStrikeAtkDef.Size = new Size(100, 23);
            txtStrikeAtkDef.TabIndex = 18;
            // 
            // txtSuperKiDef
            // 
            txtSuperKiDef.Location = new Point(120, 590);
            txtSuperKiDef.Name = "txtSuperKiDef";
            txtSuperKiDef.Size = new Size(100, 23);
            txtSuperKiDef.TabIndex = 19;
            // 
            // txtGroundSpeed
            // 
            txtGroundSpeed.Location = new Point(120, 620);
            txtGroundSpeed.Name = "txtGroundSpeed";
            txtGroundSpeed.Size = new Size(100, 23);
            txtGroundSpeed.TabIndex = 20;
            // 
            // txtAirSpeed
            // 
            txtAirSpeed.Location = new Point(120, 650);
            txtAirSpeed.Name = "txtAirSpeed";
            txtAirSpeed.Size = new Size(100, 23);
            txtAirSpeed.TabIndex = 21;
            // 
            // txtBoostSpeed
            // 
            txtBoostSpeed.Location = new Point(120, 680);
            txtBoostSpeed.Name = "txtBoostSpeed";
            txtBoostSpeed.Size = new Size(100, 23);
            txtBoostSpeed.TabIndex = 22;
            // 
            // txtDashSpeed
            // 
            txtDashSpeed.Location = new Point(120, 710);
            txtDashSpeed.Name = "txtDashSpeed";
            txtDashSpeed.Size = new Size(100, 23);
            txtDashSpeed.TabIndex = 23;
            // 
            // txtF96
            // 
            txtF96.Location = new Point(120, 740);
            txtF96.Name = "txtF96";
            txtF96.Size = new Size(100, 23);
            txtF96.TabIndex = 24;
            // 
            // txtReinforcementSkill
            // 
            txtReinforcementSkill.Location = new Point(120, 770);
            txtReinforcementSkill.Name = "txtReinforcementSkill";
            txtReinforcementSkill.Size = new Size(100, 23);
            txtReinforcementSkill.TabIndex = 25;
            // 
            // txtF104
            // 
            txtF104.Location = new Point(120, 800);
            txtF104.Name = "txtF104";
            txtF104.Size = new Size(100, 23);
            txtF104.TabIndex = 26;
            // 
            // txtRevivalHpAmount
            // 
            txtRevivalHpAmount.Location = new Point(120, 830);
            txtRevivalHpAmount.Name = "txtRevivalHpAmount";
            txtRevivalHpAmount.Size = new Size(100, 23);
            txtRevivalHpAmount.TabIndex = 27;
            // 
            // txtRevivingSpeed
            // 
            txtRevivingSpeed.Location = new Point(120, 860);
            txtRevivingSpeed.Name = "txtRevivingSpeed";
            txtRevivingSpeed.Size = new Size(100, 23);
            txtRevivingSpeed.TabIndex = 28;
            // 
            // txtF116
            // 
            txtF116.Location = new Point(120, 890);
            txtF116.Name = "txtF116";
            txtF116.Size = new Size(100, 23);
            txtF116.TabIndex = 29;
            // 
            // txtF120
            // 
            txtF120.Location = new Point(120, 920);
            txtF120.Name = "txtF120";
            txtF120.Size = new Size(100, 23);
            txtF120.TabIndex = 30;
            // 
            // txtF124
            // 
            txtF124.Location = new Point(120, 950);
            txtF124.Name = "txtF124";
            txtF124.Size = new Size(100, 23);
            txtF124.TabIndex = 31;
            // 
            // txtF128
            // 
            txtF128.Location = new Point(120, 980);
            txtF128.Name = "txtF128";
            txtF128.Size = new Size(100, 23);
            txtF128.TabIndex = 32;
            // 
            // txtF132
            // 
            txtF132.Location = new Point(120, 1010);
            txtF132.Name = "txtF132";
            txtF132.Size = new Size(100, 23);
            txtF132.TabIndex = 33;
            // 
            // txtF136
            // 
            txtF136.Location = new Point(120, 1040);
            txtF136.Name = "txtF136";
            txtF136.Size = new Size(100, 23);
            txtF136.TabIndex = 34;
            // 
            // txtI140
            // 
            txtI140.Location = new Point(120, 1070);
            txtI140.Name = "txtI140";
            txtI140.Size = new Size(100, 23);
            txtI140.TabIndex = 35;
            // 
            // txtF144
            // 
            txtF144.Location = new Point(120, 1100);
            txtF144.Name = "txtF144";
            txtF144.Size = new Size(100, 23);
            txtF144.TabIndex = 36;
            // 
            // txtF148
            // 
            txtF148.Location = new Point(120, 1130);
            txtF148.Name = "txtF148";
            txtF148.Size = new Size(100, 23);
            txtF148.TabIndex = 37;
            // 
            // txtF152
            // 
            txtF152.Location = new Point(120, 1160);
            txtF152.Name = "txtF152";
            txtF152.Size = new Size(100, 23);
            txtF152.TabIndex = 38;
            // 
            // txtF156
            // 
            txtF156.Location = new Point(120, 1190);
            txtF156.Name = "txtF156";
            txtF156.Size = new Size(100, 23);
            txtF156.TabIndex = 39;
            // 
            // txtF160
            // 
            txtF160.Location = new Point(120, 1220);
            txtF160.Name = "txtF160";
            txtF160.Size = new Size(100, 23);
            txtF160.TabIndex = 40;
            // 
            // txtF164
            // 
            txtF164.Location = new Point(120, 1250);
            txtF164.Name = "txtF164";
            txtF164.Size = new Size(100, 23);
            txtF164.TabIndex = 41;
            // 
            // txtZSoul
            // 
            txtZSoul.Location = new Point(120, 1280);
            txtZSoul.Name = "txtZSoul";
            txtZSoul.Size = new Size(100, 23);
            txtZSoul.TabIndex = 42;
            // 
            // txtI172
            // 
            txtI172.Location = new Point(120, 1310);
            txtI172.Name = "txtI172";
            txtI172.Size = new Size(100, 23);
            txtI172.TabIndex = 43;
            // 
            // txtI176
            // 
            txtI176.Location = new Point(120, 1340);
            txtI176.Name = "txtI176";
            txtI176.Size = new Size(100, 23);
            txtI176.TabIndex = 44;
            // 
            // txtF180
            // 
            txtF180.Location = new Point(120, 1370);
            txtF180.Name = "txtF180";
            txtF180.Size = new Size(100, 23);
            txtF180.TabIndex = 45;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(20, 23);
            label1.Name = "label1";
            label1.Size = new Size(50, 15);
            label1.TabIndex = 46;
            label1.Text = "Costume";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(20, 53);
            label2.Name = "label2";
            label2.Size = new Size(38, 15);
            label2.TabIndex = 47;
            label2.Text = "Preset";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(20, 83);
            label3.Name = "label3";
            label3.Size = new Size(95, 15);
            label3.TabIndex = 48;
            label3.Text = "Camera Position";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(20, 113);
            label4.Name = "label4";
            label4.Size = new Size(44, 15);
            label4.TabIndex = 49;
            label4.Text = "Health";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(20, 143);
            label5.Name = "label5";
            label5.Size = new Size(20, 15);
            label5.TabIndex = 50;
            label5.Text = "I12";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(20, 173);
            label6.Name = "label6";
            label6.Size = new Size(20, 15);
            label6.TabIndex = 51;
            label6.Text = "F20";
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(20, 203);
            label7.Name = "label7";
            label7.Size = new Size(17, 15);
            label7.TabIndex = 52;
            label7.Text = "Ki";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(20, 233);
            label8.Name = "label8";
            label8.Size = new Size(70, 15);
            label8.TabIndex = 53;
            label8.Text = "Ki Recharge";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(20, 263);
            label9.Name = "label9";
            label9.Size = new Size(20, 15);
            label9.TabIndex = 54;
            label9.Text = "I32";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(20, 293);
            label10.Name = "label10";
            label10.Size = new Size(20, 15);
            label10.TabIndex = 55;
            label10.Text = "I36";
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(20, 323);
            label11.Name = "label11";
            label11.Size = new Size(20, 15);
            label11.TabIndex = 56;
            label11.Text = "I40";
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(20, 353);
            label12.Name = "label12";
            label12.Size = new Size(54, 15);
            label12.TabIndex = 57;
            label12.Text = "Stamina";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(20, 383);
            label13.Name = "label13";
            label13.Size = new Size(100, 15);
            label13.TabIndex = 58;
            label13.Text = "Stamina Recharge";
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(20, 413);
            label14.Name = "label14";
            label14.Size = new Size(20, 15);
            label14.TabIndex = 59;
            label14.Text = "F52";
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(20, 443);
            label15.Name = "label15";
            label15.Size = new Size(20, 15);
            label15.TabIndex = 60;
            label15.Text = "F56";
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(20, 473);
            label16.Name = "label16";
            label16.Size = new Size(20, 15);
            label16.TabIndex = 61;
            label16.Text = "I60";
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new Point(20, 503);
            label17.Name = "label17";
            label17.Size = new Size(80, 15);
            label17.TabIndex = 62;
            label17.Text = "Basic Atk Def";
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Location = new Point(20, 533);
            label18.Name = "label18";
            label18.Size = new Size(70, 15);
            label18.TabIndex = 63;
            label18.Text = "Basic Ki Def";
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Location = new Point(20, 563);
            label19.Name = "label19";
            label19.Size = new Size(80, 15);
            label19.TabIndex = 64;
            label19.Text = "Strike Atk Def";
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Location = new Point(20, 593);
            label20.Name = "label20";
            label20.Size = new Size(80, 15);
            label20.TabIndex = 65;
            label20.Text = "Super Ki Def";
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Location = new Point(20, 623);
            label21.Name = "label21";
            label21.Size = new Size(80, 15);
            label21.TabIndex = 66;
            label21.Text = "Ground Speed";
            // 
            // label22
            // 
            label22.AutoSize = true;
            label22.Location = new Point(20, 653);
            label22.Name = "label22";
            label22.Size = new Size(60, 15);
            label22.TabIndex = 67;
            label22.Text = "Air Speed";
            // 
            // label23
            // 
            label23.AutoSize = true;
            label23.Location = new Point(20, 683);
            label23.Name = "label23";
            label23.Size = new Size(80, 15);
            label23.TabIndex = 68;
            label23.Text = "Boost Speed";
            // 
            // label24
            // 
            label24.AutoSize = true;
            label24.Location = new Point(20, 713);
            label24.Name = "label24";
            label24.Size = new Size(70, 15);
            label24.TabIndex = 69;
            label24.Text = "Dash Speed";
            // 
            // label25
            // 
            label25.AutoSize = true;
            label25.Location = new Point(20, 743);
            label25.Name = "label25";
            label25.Size = new Size(20, 15);
            label25.TabIndex = 70;
            label25.Text = "F96";
            // 
            // label26
            // 
            label26.AutoSize = true;
            label26.Location = new Point(20, 773);
            label26.Name = "label26";
            label26.Size = new Size(110, 15);
            label26.TabIndex = 71;
            label26.Text = "Reinforcement Skill";
            // 
            // label27
            // 
            label27.AutoSize = true;
            label27.Location = new Point(20, 803);
            label27.Name = "label27";
            label27.Size = new Size(30, 15);
            label27.TabIndex = 72;
            label27.Text = "F104";
            // 
            // label28
            // 
            label28.AutoSize = true;
            label28.Location = new Point(20, 833);
            label28.Name = "label28";
            label28.Size = new Size(100, 15);
            label28.TabIndex = 73;
            label28.Text = "Revival HP Amount";
            // 
            // label29
            // 
            label29.AutoSize = true;
            label29.Location = new Point(20, 863);
            label29.Name = "label29";
            label29.Size = new Size(90, 15);
            label29.TabIndex = 74;
            label29.Text = "Reviving Speed";
            // 
            // label30
            // 
            label30.AutoSize = true;
            label30.Location = new Point(20, 893);
            label30.Name = "label30";
            label30.Size = new Size(30, 15);
            label30.TabIndex = 75;
            label30.Text = "F116";
            // 
            // label31
            // 
            label31.AutoSize = true;
            label31.Location = new Point(20, 923);
            label31.Name = "label31";
            label31.Size = new Size(30, 15);
            label31.TabIndex = 76;
            label31.Text = "F120";
            // 
            // label32
            // 
            label32.AutoSize = true;
            label32.Location = new Point(20, 953);
            label32.Name = "label32";
            label32.Size = new Size(30, 15);
            label32.TabIndex = 77;
            label32.Text = "F124";
            // 
            // label33
            // 
            label33.AutoSize = true;
            label33.Location = new Point(20, 983);
            label33.Name = "label33";
            label33.Size = new Size(30, 15);
            label33.TabIndex = 78;
            label33.Text = "F128";
            // 
            // label34
            // 
            label34.AutoSize = true;
            label34.Location = new Point(20, 1013);
            label34.Name = "label34";
            label34.Size = new Size(30, 15);
            label34.TabIndex = 79;
            label34.Text = "F132";
            // 
            // label35
            // 
            label35.AutoSize = true;
            label35.Location = new Point(20, 1043);
            label35.Name = "label35";
            label35.Size = new Size(30, 15);
            label35.TabIndex = 80;
            label35.Text = "F136";
            // 
            // label36
            // 
            label36.AutoSize = true;
            label36.Location = new Point(20, 1073);
            label36.Name = "label36";
            label36.Size = new Size(30, 15);
            label36.TabIndex = 81;
            label36.Text = "I140";
            // 
            // label37
            // 
            label37.AutoSize = true;
            label37.Location = new Point(20, 1103);
            label37.Name = "label37";
            label37.Size = new Size(30, 15);
            label37.TabIndex = 82;
            label37.Text = "F144";
            // 
            // label38
            // 
            label38.AutoSize = true;
            label38.Location = new Point(20, 1133);
            label38.Name = "label38";
            label38.Size = new Size(30, 15);
            label38.TabIndex = 83;
            label38.Text = "F148";
            // 
            // label39
            // 
            label39.AutoSize = true;
            label39.Location = new Point(20, 1163);
            label39.Name = "label39";
            label39.Size = new Size(30, 15);
            label39.TabIndex = 84;
            label39.Text = "F152";
            // 
            // label40
            // 
            label40.AutoSize = true;
            label40.Location = new Point(20, 1193);
            label40.Name = "label40";
            label40.Size = new Size(30, 15);
            label40.TabIndex = 85;
            label40.Text = "F156";
            // 
            // label41
            // 
            label41.AutoSize = true;
            label41.Location = new Point(20, 1223);
            label41.Name = "label41";
            label41.Size = new Size(30, 15);
            label41.TabIndex = 86;
            label41.Text = "F160";
            // 
            // label42
            // 
            label42.AutoSize = true;
            label42.Location = new Point(20, 1253);
            label42.Name = "label42";
            label42.Size = new Size(30, 15);
            label42.TabIndex = 87;
            label42.Text = "F164";
            // 
            // label43
            // 
            label43.AutoSize = true;
            label43.Location = new Point(20, 1283);
            label43.Name = "label43";
            label43.Size = new Size(40, 15);
            label43.TabIndex = 88;
            label43.Text = "Z Soul";
            // 
            // label44
            // 
            label44.AutoSize = true;
            label44.Location = new Point(20, 1313);
            label44.Name = "label44";
            label44.Size = new Size(30, 15);
            label44.TabIndex = 89;
            label44.Text = "I172";
            // 
            // label45
            // 
            label45.AutoSize = true;
            label45.Location = new Point(20, 1343);
            label45.Name = "label45";
            label45.Size = new Size(30, 15);
            label45.TabIndex = 90;
            label45.Text = "I176";
            // 
            // label46
            // 
            label46.AutoSize = true;
            label46.Location = new Point(20, 1373);
            label46.Name = "label46";
            label46.Size = new Size(30, 15);
            label46.TabIndex = 91;
            label46.Text = "F180";

            // 
            // menuStrip1
            // 
            menuStrip1.Items.AddRange(new ToolStripItem[] { fileToolStripMenuItem });
            menuStrip1.Location = new Point(0, 0);
            menuStrip1.Name = "menuStrip1";
            menuStrip1.Size = new Size(706, 24);
            menuStrip1.TabIndex = 8;
            menuStrip1.Text = "menuStrip1";
            // 
            // fileToolStripMenuItem
            // 
            fileToolStripMenuItem.DropDownItems.AddRange(new ToolStripItem[] { openToolStripMenuItem, buildXVModFileToolStripMenuItem, toolStripSeparator1, sToolStripMenuItem });
            fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            fileToolStripMenuItem.Size = new Size(37, 20);
            fileToolStripMenuItem.Text = "File";
            // 
            // openToolStripMenuItem
            // 
            openToolStripMenuItem.Name = "openToolStripMenuItem";
            openToolStripMenuItem.Size = new Size(180, 22);
            openToolStripMenuItem.Text = "Open";
            openToolStripMenuItem.Click += openToolStripMenuItem_Click;
            // 
            // buildXVModFileToolStripMenuItem
            // 
            buildXVModFileToolStripMenuItem.Name = "buildXVModFileToolStripMenuItem";
            buildXVModFileToolStripMenuItem.Size = new Size(180, 22);
            buildXVModFileToolStripMenuItem.Text = "Build XVMod File";
            buildXVModFileToolStripMenuItem.Click += buildXVModFileToolStripMenuItem_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new Size(177, 6);
            // 
            // sToolStripMenuItem
            // 
            sToolStripMenuItem.Name = "sToolStripMenuItem";
            sToolStripMenuItem.Size = new Size(180, 22);
            sToolStripMenuItem.Text = "Exit";
            sToolStripMenuItem.Click += sToolStripMenuItem_Click;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Controls.Add(tabPage3);
            tabControl1.Controls.Add(tabPage4);
            tabControl1.Controls.Add(tabPage5);
            tabControl1.Controls.Add(tabPage6);
            tabControl1.Controls.Add(tabPage7);
            tabControl1.Controls.Add(tabPage8);
            tabControl1.Dock = DockStyle.Fill;
            tabControl1.Location = new Point(0, 24);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(706, 678);
            tabControl1.TabIndex = 9;
            // 
            // tabPage1
            // 
            tabPage1.BackColor = SystemColors.Control;
            tabPage1.Controls.Add(button2);
            tabPage1.Controls.Add(button1);
            tabPage1.Controls.Add(textBox2);
            tabPage1.Controls.Add(label30);
            tabPage1.Controls.Add(textBox1);
            tabPage1.Controls.Add(label27);
            tabPage1.Controls.Add(btnGenID);
            tabPage1.Controls.Add(txtCharID);
            tabPage1.Controls.Add(btnFolder);
            tabPage1.Controls.Add(txtName);
            tabPage1.Controls.Add(txtAuthor);
            tabPage1.Controls.Add(txtFolder);
            tabPage1.Controls.Add(label3);
            tabPage1.Controls.Add(label2);
            tabPage1.Controls.Add(label5);
            tabPage1.Controls.Add(label1);
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(698, 650);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "Mod Info";
            // 
            // button2
            // 
            button2.Location = new Point(318, 229);
            button2.Name = "button2";
            button2.Size = new Size(33, 23);
            button2.TabIndex = 30;
            button2.Text = "...";
            button2.UseVisualStyleBackColor = true;
            button2.Click += btnAdditionalFiles_Click;
            // 
            // button1
            // 
            button1.Location = new Point(317, 189);
            button1.Name = "button1";
            button1.Size = new Size(33, 23);
            button1.TabIndex = 30;
            button1.Text = "...";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(142, 229);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(170, 23);
            textBox2.TabIndex = 29;
            // 
            // label30
            // 
            label30.AutoSize = true;
            label30.Location = new Point(29, 233);
            label30.Name = "label30";
            label30.Size = new Size(92, 15);
            label30.TabIndex = 28;
            label30.Text = "Additional Data:";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(141, 189);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(170, 23);
            textBox1.TabIndex = 29;
            // 
            // label27
            // 
            label27.AutoSize = true;
            label27.Location = new Point(28, 193);
            label27.Name = "label27";
            label27.Size = new Size(103, 15);
            label27.TabIndex = 28;
            label27.Text = "Character Portrait:";
            // 
            // btnGenID
            // 
            btnGenID.Location = new Point(213, 110);
            btnGenID.Name = "btnGenID";
            btnGenID.Size = new Size(75, 23);
            btnGenID.TabIndex = 27;
            btnGenID.Text = "Generate";
            btnGenID.UseVisualStyleBackColor = true;
            btnGenID.Click += btnGenID_Click;
            // 
            // txtCharID
            // 
            txtCharID.CharacterCasing = CharacterCasing.Upper;
            txtCharID.Location = new Point(141, 110);
            txtCharID.MaxLength = 3;
            txtCharID.Name = "txtCharID";
            txtCharID.Size = new Size(66, 23);
            txtCharID.TabIndex = 26;
            // 
            // btnFolder
            // 
            btnFolder.Location = new Point(317, 150);
            btnFolder.Name = "btnFolder";
            btnFolder.Size = new Size(33, 23);
            btnFolder.TabIndex = 25;
            btnFolder.Text = "...";
            btnFolder.UseVisualStyleBackColor = true;
            btnFolder.Click += btnFolder_Click;
            // 
            // txtName
            // 
            txtName.Location = new Point(141, 34);
            txtName.Name = "txtName";
            txtName.Size = new Size(170, 23);
            txtName.TabIndex = 21;
            // 
            // txtAuthor
            // 
            txtAuthor.Location = new Point(141, 70);
            txtAuthor.Name = "txtAuthor";
            txtAuthor.Size = new Size(170, 23);
            txtAuthor.TabIndex = 23;
            // 
            // txtFolder
            // 
            txtFolder.Location = new Point(141, 150);
            txtFolder.Name = "txtFolder";
            txtFolder.Size = new Size(170, 23);
            txtFolder.TabIndex = 24;
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Location = new Point(27, 37);
            label3.Name = "label3";
            label3.Size = new Size(70, 15);
            label3.TabIndex = 16;
            label3.Text = "Mod Name:";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(27, 73);
            label2.Name = "label2";
            label2.Size = new Size(75, 15);
            label2.TabIndex = 18;
            label2.Text = "Mod Author:";
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Location = new Point(56, 113);
            label5.Name = "label5";
            label5.Size = new Size(49, 15);
            label5.TabIndex = 19;
            label5.Text = "Char ID:";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(28, 154);
            label1.Name = "label1";
            label1.Size = new Size(97, 15);
            label1.TabIndex = 20;
            label1.Text = "Character Folder:";
            // 
            // tabPage2
            // 
            tabPage2.BackColor = SystemColors.Control;
            tabPage2.Controls.Add(txtAuraID);
            tabPage2.Controls.Add(cbAuraGlare);
            tabPage2.Controls.Add(label6);
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(698, 650);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "AUR";
            // 
            // txtAuraID
            // 
            txtAuraID.Location = new Point(99, 28);
            txtAuraID.Name = "txtAuraID";
            txtAuraID.Size = new Size(100, 23);
            txtAuraID.TabIndex = 3;
            txtAuraID.KeyPress += txtAuraID_KeyPress;
            // 
            // cbAuraGlare
            // 
            cbAuraGlare.AutoSize = true;
            cbAuraGlare.Location = new Point(205, 32);
            cbAuraGlare.Name = "cbAuraGlare";
            cbAuraGlare.Size = new Size(53, 19);
            cbAuraGlare.TabIndex = 2;
            cbAuraGlare.Text = "Glare";
            cbAuraGlare.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Location = new Point(44, 33);
            label6.Name = "label6";
            label6.Size = new Size(49, 15);
            label6.TabIndex = 1;
            label6.Text = "Aura ID:";
            // 
            // tabPage3
            // 
            tabPage3.BackColor = SystemColors.Control;
            tabPage3.Controls.Add(txtBAI);
            tabPage3.Controls.Add(label13);
            tabPage3.Controls.Add(txtBCM);
            tabPage3.Controls.Add(label12);
            tabPage3.Controls.Add(txtBAC);
            tabPage3.Controls.Add(label11);
            tabPage3.Controls.Add(txtCAMEAN);
            tabPage3.Controls.Add(label10);
            tabPage3.Controls.Add(txtFCEEAN);
            tabPage3.Controls.Add(label9);
            tabPage3.Controls.Add(txtEAN);
            tabPage3.Controls.Add(label8);
            tabPage3.Controls.Add(txtBCS);
            tabPage3.Controls.Add(label7);
            tabPage3.Location = new Point(4, 24);
            tabPage3.Name = "tabPage3";
            tabPage3.Padding = new Padding(3);
            tabPage3.Size = new Size(698, 650);
            tabPage3.TabIndex = 2;
            tabPage3.Text = "CMS";
            // 
            // txtBAI
            // 
            txtBAI.Location = new Point(111, 213);
            txtBAI.Name = "txtBAI";
            txtBAI.Size = new Size(218, 23);
            txtBAI.TabIndex = 5;
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Location = new Point(39, 216);
            label13.Name = "label13";
            label13.Size = new Size(28, 15);
            label13.TabIndex = 4;
            label13.Text = "BAI:";
            // 
            // txtBCM
            // 
            txtBCM.Location = new Point(111, 184);
            txtBCM.Name = "txtBCM";
            txtBCM.Size = new Size(218, 23);
            txtBCM.TabIndex = 5;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Location = new Point(39, 187);
            label12.Name = "label12";
            label12.Size = new Size(36, 15);
            label12.TabIndex = 4;
            label12.Text = "BCM:";
            // 
            // txtBAC
            // 
            txtBAC.Location = new Point(111, 155);
            txtBAC.Name = "txtBAC";
            txtBAC.Size = new Size(218, 23);
            txtBAC.TabIndex = 5;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Location = new Point(39, 158);
            label11.Name = "label11";
            label11.Size = new Size(33, 15);
            label11.TabIndex = 4;
            label11.Text = "BAC:";
            // 
            // txtCAMEAN
            // 
            txtCAMEAN.Location = new Point(111, 126);
            txtCAMEAN.Name = "txtCAMEAN";
            txtCAMEAN.Size = new Size(218, 23);
            txtCAMEAN.TabIndex = 5;
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Location = new Point(39, 129);
            label10.Name = "label10";
            label10.Size = new Size(65, 15);
            label10.TabIndex = 4;
            label10.Text = "CAM_EAN:";
            // 
            // txtFCEEAN
            // 
            txtFCEEAN.Location = new Point(111, 97);
            txtFCEEAN.Name = "txtFCEEAN";
            txtFCEEAN.Size = new Size(218, 23);
            txtFCEEAN.TabIndex = 5;
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Location = new Point(39, 100);
            label9.Name = "label9";
            label9.Size = new Size(58, 15);
            label9.TabIndex = 4;
            label9.Text = "FCE_EAN:";
            // 
            // txtEAN
            // 
            txtEAN.Location = new Point(111, 68);
            txtEAN.Name = "txtEAN";
            txtEAN.Size = new Size(218, 23);
            txtEAN.TabIndex = 5;
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Location = new Point(39, 71);
            label8.Name = "label8";
            label8.Size = new Size(33, 15);
            label8.TabIndex = 4;
            label8.Text = "EAN:";
            // 
            // txtBCS
            // 
            txtBCS.Location = new Point(111, 39);
            txtBCS.Name = "txtBCS";
            txtBCS.Size = new Size(218, 23);
            txtBCS.TabIndex = 5;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Location = new Point(39, 42);
            label7.Name = "label7";
            label7.Size = new Size(31, 15);
            label7.TabIndex = 4;
            label7.Text = "BCS:";
            // 
            // tabPage4
            // 
            tabPage4.BackColor = SystemColors.Control;
            tabPage4.Controls.Add(txtCSO4);
            tabPage4.Controls.Add(label14);
            tabPage4.Controls.Add(txtCSO3);
            tabPage4.Controls.Add(label15);
            tabPage4.Controls.Add(txtCSO2);
            tabPage4.Controls.Add(label16);
            tabPage4.Controls.Add(txtCSO1);
            tabPage4.Controls.Add(label17);
            tabPage4.Location = new Point(4, 24);
            tabPage4.Name = "tabPage4";
            tabPage4.Padding = new Padding(3);
            tabPage4.Size = new Size(698, 650);
            tabPage4.TabIndex = 3;
            tabPage4.Text = "CSO";
            // 
            // txtCSO4
            // 
            txtCSO4.Location = new Point(101, 138);
            txtCSO4.Name = "txtCSO4";
            txtCSO4.Size = new Size(218, 23);
            txtCSO4.TabIndex = 10;
            // 
            // label14
            // 
            label14.AutoSize = true;
            label14.Location = new Point(70, 141);
            label14.Name = "label14";
            label14.Size = new Size(16, 15);
            label14.TabIndex = 6;
            label14.Text = "4:";
            // 
            // txtCSO3
            // 
            txtCSO3.Location = new Point(101, 109);
            txtCSO3.Name = "txtCSO3";
            txtCSO3.Size = new Size(218, 23);
            txtCSO3.TabIndex = 11;
            // 
            // label15
            // 
            label15.AutoSize = true;
            label15.Location = new Point(70, 112);
            label15.Name = "label15";
            label15.Size = new Size(16, 15);
            label15.TabIndex = 7;
            label15.Text = "3:";
            // 
            // txtCSO2
            // 
            txtCSO2.Location = new Point(101, 80);
            txtCSO2.Name = "txtCSO2";
            txtCSO2.Size = new Size(218, 23);
            txtCSO2.TabIndex = 12;
            // 
            // label16
            // 
            label16.AutoSize = true;
            label16.Location = new Point(70, 83);
            label16.Name = "label16";
            label16.Size = new Size(16, 15);
            label16.TabIndex = 8;
            label16.Text = "2:";
            // 
            // txtCSO1
            // 
            txtCSO1.Location = new Point(101, 51);
            txtCSO1.Name = "txtCSO1";
            txtCSO1.Size = new Size(218, 23);
            txtCSO1.TabIndex = 13;
            // 
            // label17
            // 
            label17.AutoSize = true;
            label17.Location = new Point(70, 54);
            label17.Name = "label17";
            label17.Size = new Size(16, 15);
            label17.TabIndex = 9;
            label17.Text = "1:";
            // 
            // tabPage5
            // 
            tabPage5.BackColor = SystemColors.Control;
            tabPage5.Controls.Add(label23);
            tabPage5.Controls.Add(label22);
            tabPage5.Controls.Add(label21);
            tabPage5.Controls.Add(label24);
            tabPage5.Controls.Add(label20);
            tabPage5.Controls.Add(label19);
            tabPage5.Controls.Add(label18);
            tabPage5.Controls.Add(cbEvasive);
            tabPage5.Controls.Add(cbUltimate2);
            tabPage5.Controls.Add(cbSuper4);
            tabPage5.Controls.Add(cbUltimate1);
            tabPage5.Controls.Add(cbSuper3);
            tabPage5.Controls.Add(cbSuper2);
            tabPage5.Controls.Add(cbSuper1);
            tabPage5.Location = new Point(4, 24);
            tabPage5.Name = "tabPage5";
            tabPage5.Padding = new Padding(3);
            tabPage5.Size = new Size(698, 650);
            tabPage5.TabIndex = 4;
            tabPage5.Text = "CUS";
            // 
            // label23
            // 
            label23.AutoSize = true;
            label23.Location = new Point(73, 217);
            label23.Name = "label23";
            label23.Size = new Size(48, 15);
            label23.TabIndex = 1;
            label23.Text = "Evasive:";
            // 
            // label22
            // 
            label22.AutoSize = true;
            label22.Location = new Point(58, 191);
            label22.Name = "label22";
            label22.Size = new Size(64, 15);
            label22.TabIndex = 1;
            label22.Text = "Ultimate 2:";
            // 
            // label21
            // 
            label21.AutoSize = true;
            label21.Location = new Point(58, 162);
            label21.Name = "label21";
            label21.Size = new Size(64, 15);
            label21.TabIndex = 1;
            label21.Text = "Ultimate 1:";
            // 
            // label24
            // 
            label24.AutoSize = true;
            label24.Location = new Point(78, 133);
            label24.Name = "label24";
            label24.Size = new Size(49, 15);
            label24.TabIndex = 1;
            label24.Text = "Super 4:";
            // 
            // label20
            // 
            label20.AutoSize = true;
            label20.Location = new Point(78, 103);
            label20.Name = "label20";
            label20.Size = new Size(49, 15);
            label20.TabIndex = 1;
            label20.Text = "Super 3:";
            // 
            // label19
            // 
            label19.AutoSize = true;
            label19.Location = new Point(78, 74);
            label19.Name = "label19";
            label19.Size = new Size(49, 15);
            label19.TabIndex = 1;
            label19.Text = "Super 2:";
            // 
            // label18
            // 
            label18.AutoSize = true;
            label18.Location = new Point(78, 45);
            label18.Name = "label18";
            label18.Size = new Size(49, 15);
            label18.TabIndex = 1;
            label18.Text = "Super 1:";
            // 
            // cbEvasive
            // 
            cbEvasive.FormattingEnabled = true;
            cbEvasive.Location = new Point(128, 217);
            cbEvasive.Name = "cbEvasive";
            cbEvasive.Size = new Size(436, 23);
            cbEvasive.TabIndex = 0;
            // 
            // cbUltimate2
            // 
            cbUltimate2.FormattingEnabled = true;
            cbUltimate2.Location = new Point(128, 188);
            cbUltimate2.Name = "cbUltimate2";
            cbUltimate2.Size = new Size(436, 23);
            cbUltimate2.TabIndex = 0;
            // 
            // cbSuper4
            // 
            cbSuper4.FormattingEnabled = true;
            cbSuper4.Location = new Point(128, 130);
            cbSuper4.Name = "cbSuper4";
            cbSuper4.Size = new Size(436, 23);
            cbSuper4.TabIndex = 0;
            // 
            // cbUltimate1
            // 
            cbUltimate1.FormattingEnabled = true;
            cbUltimate1.Location = new Point(128, 159);
            cbUltimate1.Name = "cbUltimate1";
            cbUltimate1.Size = new Size(436, 23);
            cbUltimate1.TabIndex = 0;
            // 
            // cbSuper3
            // 
            cbSuper3.FormattingEnabled = true;
            cbSuper3.Location = new Point(128, 100);
            cbSuper3.Name = "cbSuper3";
            cbSuper3.Size = new Size(436, 23);
            cbSuper3.TabIndex = 0;
            // 
            // cbSuper2
            // 
            cbSuper2.FormattingEnabled = true;
            cbSuper2.Location = new Point(128, 71);
            cbSuper2.Name = "cbSuper2";
            cbSuper2.Size = new Size(436, 23);
            cbSuper2.TabIndex = 0;
            // 
            // cbSuper1
            // 
            cbSuper1.FormattingEnabled = true;
            cbSuper1.Location = new Point(128, 42);
            cbSuper1.Name = "cbSuper1";
            cbSuper1.Size = new Size(436, 23);
            cbSuper1.TabIndex = 0;
            // 
            // panelPSC
            // 
            panelPSC.AutoScroll = true;
            panelPSC.Dock = DockStyle.Fill;
            panelPSC.Location = new Point(3, 3);
            panelPSC.Name = "panelPSC";
            panelPSC.Size = new Size(692, 644);
            panelPSC.TabIndex = 0;
            
            // Add all PSC controls to the panel
            panelPSC.Controls.Add(txtF180);
            panelPSC.Controls.Add(txtI176);
            panelPSC.Controls.Add(txtI172);
            panelPSC.Controls.Add(txtZSoul);
            panelPSC.Controls.Add(txtF164);
            panelPSC.Controls.Add(txtF160);
            panelPSC.Controls.Add(txtF156);
            panelPSC.Controls.Add(txtF152);
            panelPSC.Controls.Add(txtF148);
            panelPSC.Controls.Add(txtF144);
            panelPSC.Controls.Add(txtI140);
            panelPSC.Controls.Add(txtF136);
            panelPSC.Controls.Add(txtF132);
            panelPSC.Controls.Add(txtF128);
            panelPSC.Controls.Add(txtF124);
            panelPSC.Controls.Add(txtF120);
            panelPSC.Controls.Add(txtF116);
            panelPSC.Controls.Add(txtRevivingSpeed);
            panelPSC.Controls.Add(txtRevivalHpAmount);
            panelPSC.Controls.Add(txtF104);
            panelPSC.Controls.Add(txtReinforcementSkill);
            panelPSC.Controls.Add(txtF96);
            panelPSC.Controls.Add(txtDashSpeed);
            panelPSC.Controls.Add(txtBoostSpeed);
            panelPSC.Controls.Add(txtAirSpeed);
            panelPSC.Controls.Add(txtGroundSpeed);
            panelPSC.Controls.Add(txtSuperKiDef);
            panelPSC.Controls.Add(txtStrikeAtkDef);
            panelPSC.Controls.Add(txtBasicKiDef);
            panelPSC.Controls.Add(txtBasicAtkDef);
            panelPSC.Controls.Add(txtI60);
            panelPSC.Controls.Add(txtF56);
            panelPSC.Controls.Add(txtF52);
            panelPSC.Controls.Add(txtStaminaRecharge);
            panelPSC.Controls.Add(txtStamina);
            panelPSC.Controls.Add(txtI40);
            panelPSC.Controls.Add(txtI36);
            panelPSC.Controls.Add(txtI32);
            panelPSC.Controls.Add(txtKiRecharge);
            panelPSC.Controls.Add(txtKi);
            panelPSC.Controls.Add(txtF20);
            panelPSC.Controls.Add(txtI12);
            panelPSC.Controls.Add(txtHealth);
            panelPSC.Controls.Add(txtCameraPosition);
            panelPSC.Controls.Add(txtPreset);
            panelPSC.Controls.Add(txtCostume);
            panelPSC.Controls.Add(label46);
            panelPSC.Controls.Add(label45);
            panelPSC.Controls.Add(label44);
            panelPSC.Controls.Add(label43);
            panelPSC.Controls.Add(label42);
            panelPSC.Controls.Add(label41);
            panelPSC.Controls.Add(label40);
            panelPSC.Controls.Add(label39);
            panelPSC.Controls.Add(label38);
            panelPSC.Controls.Add(label37);
            panelPSC.Controls.Add(label36);
            panelPSC.Controls.Add(label35);
            panelPSC.Controls.Add(label34);
            panelPSC.Controls.Add(label33);
            panelPSC.Controls.Add(label32);
            panelPSC.Controls.Add(label31);
            panelPSC.Controls.Add(label30);
            panelPSC.Controls.Add(label29);
            panelPSC.Controls.Add(label28);
            panelPSC.Controls.Add(label27);
            panelPSC.Controls.Add(label26);
            panelPSC.Controls.Add(label25);
            panelPSC.Controls.Add(label24);
            panelPSC.Controls.Add(label23);
            panelPSC.Controls.Add(label22);
            panelPSC.Controls.Add(label21);
            panelPSC.Controls.Add(label20);
            panelPSC.Controls.Add(label19);
            panelPSC.Controls.Add(label18);
            panelPSC.Controls.Add(label17);
            panelPSC.Controls.Add(label16);
            panelPSC.Controls.Add(label15);
            panelPSC.Controls.Add(label14);
            panelPSC.Controls.Add(label13);
            panelPSC.Controls.Add(label12);
            panelPSC.Controls.Add(label11);
            panelPSC.Controls.Add(label10);
            panelPSC.Controls.Add(label9);
            panelPSC.Controls.Add(label8);
            panelPSC.Controls.Add(label7);
            panelPSC.Controls.Add(label6);
            panelPSC.Controls.Add(label5);
            panelPSC.Controls.Add(label4);
            panelPSC.Controls.Add(label3);
            panelPSC.Controls.Add(label2);
            panelPSC.Controls.Add(label1);
            // 
            // tabPage6
            // 
            tabPage6.BackColor = SystemColors.Control;
            tabPage6.Controls.Add(panelPSC);
            tabPage6.Location = new Point(4, 24);
            tabPage6.Name = "tabPage6";
            tabPage6.Padding = new Padding(3);
            tabPage6.Size = new Size(698, 650);
            tabPage6.TabIndex = 5;
            tabPage6.Text = "PSC";
            // 
            // tabPage7
            // 
            tabPage7.BackColor = SystemColors.Control;
            tabPage7.Controls.Add(label26);
            tabPage7.Controls.Add(label25);
            tabPage7.Controls.Add(txtMSG2);
            tabPage7.Controls.Add(txtMSG1);
            tabPage7.Location = new Point(4, 24);
            tabPage7.Name = "tabPage7";
            tabPage7.Padding = new Padding(3);
            tabPage7.Size = new Size(698, 650);
            tabPage7.TabIndex = 6;
            tabPage7.Text = "MSG";
            // 
            // label26
            // 
            label26.AutoSize = true;
            label26.Location = new Point(37, 70);
            label26.Name = "label26";
            label26.Size = new Size(93, 15);
            label26.TabIndex = 1;
            label26.Text = "Costume Name:";
            // 
            // label25
            // 
            label25.AutoSize = true;
            label25.Location = new Point(37, 41);
            label25.Name = "label25";
            label25.Size = new Size(96, 15);
            label25.TabIndex = 1;
            label25.Text = "Character Name:";
            // 
            // txtMSG2
            // 
            txtMSG2.Location = new Point(139, 67);
            txtMSG2.Name = "txtMSG2";
            txtMSG2.Size = new Size(248, 23);
            txtMSG2.TabIndex = 0;
            // 
            // txtMSG1
            // 
            txtMSG1.Location = new Point(139, 38);
            txtMSG1.Name = "txtMSG1";
            txtMSG1.Size = new Size(248, 23);
            txtMSG1.TabIndex = 0;
            // 
            // tabPage8
            // 
            tabPage8.BackColor = SystemColors.Control;
            tabPage8.Controls.Add(txtVOX2);
            tabPage8.Controls.Add(txtVOX1);
            tabPage8.Controls.Add(label29);
            tabPage8.Controls.Add(label28);
            tabPage8.Location = new Point(4, 24);
            tabPage8.Name = "tabPage8";
            tabPage8.Padding = new Padding(3);
            tabPage8.Size = new Size(698, 650);
            tabPage8.TabIndex = 7;
            tabPage8.Text = "VOX";
            // 
            // txtVOX2
            // 
            txtVOX2.Location = new Point(99, 51);
            txtVOX2.Name = "txtVOX2";
            txtVOX2.Size = new Size(47, 23);
            txtVOX2.TabIndex = 1;
            // 
            // txtVOX1
            // 
            txtVOX1.Location = new Point(46, 51);
            txtVOX1.Name = "txtVOX1";
            txtVOX1.Size = new Size(47, 23);
            txtVOX1.TabIndex = 1;
            // 
            // label29
            // 
            label29.AutoSize = true;
            label29.Location = new Point(99, 33);
            label29.Name = "label29";
            label29.Size = new Size(47, 15);
            label29.TabIndex = 0;
            label29.Text = "Voice 2:";
            // 
            // label28
            // 
            label28.AutoSize = true;
            label28.Location = new Point(46, 33);
            label28.Name = "label28";
            label28.Size = new Size(47, 15);
            label28.TabIndex = 0;
            label28.Text = "Voice 1:";
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(706, 702);
            Controls.Add(tabControl1);
            Controls.Add(menuStrip1);
            FormBorderStyle = FormBorderStyle.FixedSingle;
            Icon = (Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            Name = "Form1";
            Text = "XVCharaCreator";
            FormClosing += Form1_FormClosing;
            Load += Form1_Load;
            menuStrip1.ResumeLayout(false);
            menuStrip1.PerformLayout();
            tabControl1.ResumeLayout(false);
            tabPage1.ResumeLayout(false);
            tabPage1.PerformLayout();
            tabPage2.ResumeLayout(false);
            tabPage2.PerformLayout();
            tabPage3.ResumeLayout(false);
            tabPage3.PerformLayout();
            tabPage4.ResumeLayout(false);
            tabPage4.PerformLayout();
            tabPage5.ResumeLayout(false);
            tabPage5.PerformLayout();
            tabPage7.ResumeLayout(false);
            tabPage7.PerformLayout();
            tabPage8.ResumeLayout(false);
            tabPage8.PerformLayout();
            tabPage6.ResumeLayout(false);
            tabPage6.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private MenuStrip menuStrip1;
        private ToolStripMenuItem fileToolStripMenuItem;
        private ToolStripMenuItem buildXVModFileToolStripMenuItem;
        private ToolStripSeparator toolStripSeparator1;
        private ToolStripMenuItem sToolStripMenuItem;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private Button btnGenID;
        private TextBox txtCharID;
        private Button btnFolder;
        private TextBox txtName;
        private TextBox txtAuthor;
        private TextBox txtFolder;
        private Label label3;
        private Label label2;
        private Label label5;
        private Label label1;
        private TabPage tabPage2;
        private TabPage tabPage3;
        private TabPage tabPage4;
        private TabPage tabPage5;
        private TabPage tabPage6;
        private Panel panelPSC;
        private TabPage tabPage7;
        private CheckBox cbAuraGlare;
        private Label label6;
        private TextBox txtAuraID;
        private TextBox txtBAC;
        private Label label11;
        private TextBox txtCAMEAN;
        private Label label10;
        private TextBox txtFCEEAN;
        private Label label9;
        private TextBox txtEAN;
        private Label label8;
        private TextBox txtBCS;
        private Label label7;
        private TextBox txtBAI;
        private Label label13;
        private TextBox txtBCM;
        private Label label12;
        private TextBox txtCSO4;
        private Label label14;
        private TextBox txtCSO3;
        private Label label15;
        private TextBox txtCSO2;
        private Label label16;
        private TextBox txtCSO1;
        private Label label17;
        private Label label20;
        private Label label19;
        private Label label18;
        private ComboBox cbEvasive;
        private ComboBox cbUltimate2;
        private ComboBox cbUltimate1;
        private ComboBox cbSuper3;
        private ComboBox cbSuper2;
        private ComboBox cbSuper1;
        private Label label23;
        private Label label22;
        private Label label21;
        private Label label24;
        private ComboBox cbSuper4;
        private Label label25;
        private TextBox txtMSG2;
        private TextBox txtMSG1;
        private Label label26;
        private Button button1;
        private TextBox textBox1;
        private Label label27;
        private TabPage tabPage8;
        private Label label28;
        private TextBox txtVOX2;
        private TextBox txtVOX1;
        private Label label29;
        private ToolStripMenuItem openToolStripMenuItem;
        private Button button2;
        private TextBox textBox2;
        private Label label30;
        private TextBox txtCostume;
        private TextBox txtPreset;
        private TextBox txtCameraPosition;
        private TextBox txtHealth;
        private TextBox txtI12;
        private TextBox txtF20;
        private TextBox txtKi;
        private TextBox txtKiRecharge;
        private TextBox txtI32;
        private TextBox txtI36;
        private TextBox txtI40;
        private TextBox txtStamina;
        private TextBox txtStaminaRecharge;
        private TextBox txtF52;
        private TextBox txtF56;
        private TextBox txtI60;
        private TextBox txtBasicAtkDef;
        private TextBox txtBasicKiDef;
        private TextBox txtStrikeAtkDef;
        private TextBox txtSuperKiDef;
        private TextBox txtGroundSpeed;
        private TextBox txtAirSpeed;
        private TextBox txtBoostSpeed;
        private TextBox txtDashSpeed;
        private TextBox txtF96;
        private TextBox txtReinforcementSkill;
        private TextBox txtF104;
        private TextBox txtRevivalHpAmount;
        private TextBox txtRevivingSpeed;
        private TextBox txtF116;
        private TextBox txtF120;
        private TextBox txtF124;
        private TextBox txtF128;
        private TextBox txtF132;
        private TextBox txtF136;
        private TextBox txtI140;
        private TextBox txtF144;
        private TextBox txtF148;
        private TextBox txtF152;
        private TextBox txtF156;
        private TextBox txtF160;
        private TextBox txtF164;
        private TextBox txtZSoul;
        private TextBox txtI172;
        private TextBox txtI176;
        private TextBox txtF180;
        private Label label4;
        private Label label31;
        private Label label32;
        private Label label33;
        private Label label34;
        private Label label35;
        private Label label36;
        private Label label37;
        private Label label38;
        private Label label39;
        private Label label40;
        private Label label41;
        private Label label42;
        private Label label43;
        private Label label44;
        private Label label45;
        private Label label46;


    }
}