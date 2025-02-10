using System.IO.Compression;
using System.Xml;
using XVCharaCreator.Properties;
using XVReborn;
using System.Xml.Linq;

namespace XVCharaCreator
{

    public partial class Form1 : Form
    {
        CharSkill CS = new CharSkill();
        public Form1()
        {
            // Initialize the TextBox controls
            this.txtCostume = new TextBox();
            this.txtPreset = new TextBox();
            this.txtCameraPosition = new TextBox();
            this.txtHealth = new TextBox();
            this.txtI12 = new TextBox();
            this.txtF20 = new TextBox();
            this.txtKi = new TextBox();
            this.txtKiRecharge = new TextBox();
            this.txtI32 = new TextBox();
            this.txtI36 = new TextBox();
            this.txtI40 = new TextBox();
            this.txtStamina = new TextBox();
            this.txtStaminaRecharge = new TextBox();
            this.txtF52 = new TextBox();
            this.txtF56 = new TextBox();
            this.txtI60 = new TextBox();
            this.txtBasicAtkDef = new TextBox();
            this.txtBasicKiDef = new TextBox();
            this.txtStrikeAtkDef = new TextBox();
            this.txtSuperKiDef = new TextBox();
            this.txtGroundSpeed = new TextBox();
            this.txtAirSpeed = new TextBox();
            this.txtBoostSpeed = new TextBox();
            this.txtDashSpeed = new TextBox();
            this.txtF96 = new TextBox();
            this.txtReinforcementSkill = new TextBox();
            this.txtF104 = new TextBox();
            this.txtRevivalHpAmount = new TextBox();
            this.txtRevivingSpeed = new TextBox();
            this.txtF116 = new TextBox();
            this.txtF120 = new TextBox();
            this.txtF124 = new TextBox();
            this.txtF128 = new TextBox();
            this.txtF132 = new TextBox();
            this.txtF136 = new TextBox();
            this.txtI140 = new TextBox();
            this.txtF144 = new TextBox();
            this.txtF148 = new TextBox();
            this.txtF152 = new TextBox();
            this.txtF156 = new TextBox();
            this.txtF160 = new TextBox();
            this.txtF164 = new TextBox();
            this.txtZSoul = new TextBox();
            this.txtI172 = new TextBox();
            this.txtI176 = new TextBox();
            this.txtF180 = new TextBox();

            // Initialize other components
            InitializeComponent();

            TableLayoutPanel tableLayoutPanel = new TableLayoutPanel();
            VScrollBar vbar = new VScrollBar();
            Panel scrollPanel = new Panel();

            vbar.Dock = DockStyle.Right;
            vbar.Width = 20; // Adjust width as needed
            vbar.Minimum = 0;
            vbar.Maximum = 100; // Adjust based on the total height of the content
            vbar.LargeChange = 10;
            vbar.SmallChange = 1;

            // Create a container panel to wrap the TableLayoutPanel and VScrollBar
            scrollPanel.Dock = DockStyle.Fill;
            scrollPanel.Controls.Add(tableLayoutPanel);

            tableLayoutPanel.RowCount = 46; // Adjust according to the number of rows
            tableLayoutPanel.ColumnCount = 6; // Labels in the first column, textboxes in the second column
            tableLayoutPanel.Location = new Point(0, 0);
            tableLayoutPanel.Size = new Size(1800, 1800); // Adjust size as needed

            // Add rows dynamically, assuming you have an array of textboxes and labels
            string[] labels = {
        "Costume", "Preset", "CameraPosition", "Health", "I12", "F20", "Ki", "KiRecharge",
        "I32", "I36", "I40", "Stamina", "StaminaRecharge", "F52", "F56", "I60", "BasicAtkDef",
        "BasicKiDef", "StrikeAtkDef", "SuperKiDef", "GroundSpeed", "AirSpeed", "BoostSpeed",
        "DashSpeed", "F96", "ReinforcementSkill", "F104", "RevivalHpAmount", "RevivingSpeed",
        "F116", "F120", "F124", "F128", "F132", "F136", "I140", "F144", "F148", "F152", "F156",
        "F160", "F164", "ZSoul", "I172", "I176", "F180"
    };

            TextBox[] textboxes = {
        txtCostume, txtPreset, txtCameraPosition, txtHealth, txtI12, txtF20, txtKi, txtKiRecharge,
        txtI32, txtI36, txtI40, txtStamina, txtStaminaRecharge, txtF52, txtF56, txtI60, txtBasicAtkDef,
        txtBasicKiDef, txtStrikeAtkDef, txtSuperKiDef, txtGroundSpeed, txtAirSpeed, txtBoostSpeed,
        txtDashSpeed, txtF96, txtReinforcementSkill, txtF104, txtRevivalHpAmount, txtRevivingSpeed,
        txtF116, txtF120, txtF124, txtF128, txtF132, txtF136, txtI140, txtF144, txtF148, txtF152,
        txtF156, txtF160, txtF164, txtZSoul, txtI172, txtI176, txtF180
    };

            for (int i = 0; i < labels.Length; i++)
            {
                tableLayoutPanel.Controls.Add(new Label() { Text = labels[i] }, 0, i);
                tableLayoutPanel.Controls.Add(textboxes[i], 1, i);
            }

            // Adjust the maximum value of the scrollbar dynamically based on content size
            vbar.Maximum = tableLayoutPanel.Height - scrollPanel.ClientSize.Height;
            vbar.LargeChange = scrollPanel.ClientSize.Height;

            // ScrollBar event handler to scroll the content
            vbar.Scroll += (sender, e) =>
            {
                tableLayoutPanel.Location = new Point(0, -vbar.Value); // Move the table layout based on the scrollbar's position
            };

            tabPage6.Controls.Add(scrollPanel);
            tabPage6.Controls.Add(vbar);
        }

        private void buildXVModFileToolStripMenuItem_Click(object sender, EventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Title = "Save Mod File";
            sfd.Filter = "Xenoverse Mod Files (*.xvmod)|*.xvmod";

            if (txtName.Text.Length > 0 && txtAuthor.Text.Length > 0
                && Directory.Exists(txtFolder.Text)
                && sfd.ShowDialog() == DialogResult.OK
                && File.Exists(textBox1.Text))
            {
                // Specify the path where you want to save the XML file
                string xmlFilePath = "./XVCharaCreatorTemp/xvmod.xml";

                if (Directory.Exists("./XVCharaCreatorTemp"))
                    Directory.Delete("./XVCharaCreatorTemp", true);
                Directory.CreateDirectory("./XVCharaCreatorTemp");

                // Create an XmlWriterSettings instance for formatting the XML
                XmlWriterSettings settings = new XmlWriterSettings
                {
                    Indent = true,
                    IndentChars = "    ", // Use four spaces for indentation
                };

                // Create the XmlWriter and write the XML content
                using (XmlWriter writer = XmlWriter.Create(xmlFilePath, settings))
                {
                    writer.WriteStartDocument();
                    writer.WriteStartElement("XVMOD");
                    writer.WriteAttributeString("type", "ADDED_CHARACTER");

                    WriteElementWithValue(writer, "MOD_NAME", txtName.Text);
                    WriteElementWithValue(writer, "MOD_AUTHOR", txtAuthor.Text);


                    // Let's start adding the actual character attributes (AUR, CMS, CSO, etc...)
                    WriteElementWithValue(writer, "AUR_ID", txtAuraID.Text);
                    if (cbAuraGlare.Checked)
                        WriteElementWithValue(writer, "AUR_GLARE", "1");
                    else
                        WriteElementWithValue(writer, "AUR_GLARE", "0");

                    WriteElementWithValue(writer, "CMS_BCS", txtBCS.Text);
                    WriteElementWithValue(writer, "CMS_EAN", txtEAN.Text);
                    WriteElementWithValue(writer, "CMS_FCE_EAN", txtFCEEAN.Text);
                    WriteElementWithValue(writer, "CMS_CAM_EAN", txtCAMEAN.Text);
                    WriteElementWithValue(writer, "CMS_BAC", txtBAC.Text);
                    WriteElementWithValue(writer, "CMS_BCM", txtBCM.Text);
                    WriteElementWithValue(writer, "CMS_BAI", txtBAI.Text);

                    WriteElementWithValue(writer, "CSO_1", txtCSO1.Text);
                    WriteElementWithValue(writer, "CSO_2", txtCSO2.Text);
                    WriteElementWithValue(writer, "CSO_3", txtCSO3.Text);
                    WriteElementWithValue(writer, "CSO_4", txtCSO4.Text);

                    SetComboBoxValueSuper(writer, cbSuper1, "CUS_SUPER_1");
                    SetComboBoxValueSuper(writer, cbSuper2, "CUS_SUPER_2");
                    SetComboBoxValueSuper(writer, cbSuper3, "CUS_SUPER_3");
                    SetComboBoxValueSuper(writer, cbSuper4, "CUS_SUPER_4");
                    SetComboBoxValueUltimate(writer, cbUltimate1, "CUS_ULTIMATE_1");
                    SetComboBoxValueUltimate(writer, cbUltimate2, "CUS_ULTIMATE_2");
                    SetComboBoxValueEvasive(writer, cbEvasive, "CUS_EVASIVE");


                    WriteElementWithValue(writer, "PSC_COSTUME", txtCostume.Text);
                    WriteElementWithValue(writer, "PSC_PRESET", txtPreset.Text);
                    WriteElementWithValue(writer, "PSC_CAMERA_POSITION", txtCameraPosition.Text);
                    WriteElementWithValue(writer, "PSC_HEALTH", txtHealth.Text);
                    WriteElementWithValue(writer, "PSC_I_12", txtI12.Text);
                    WriteElementWithValue(writer, "PSC_F_20", txtF20.Text);

                    WriteElementWithValue(writer, "PSC_KI", txtKi.Text);
                    WriteElementWithValue(writer, "PSC_KI_RECHARGE", txtKiRecharge.Text);
                    WriteElementWithValue(writer, "PSC_I_32", txtI32.Text);
                    WriteElementWithValue(writer, "PSC_I_36", txtI36.Text);
                    WriteElementWithValue(writer, "PSC_I_40", txtI40.Text);
                    WriteElementWithValue(writer, "PSC_STAMINA", txtStamina.Text);

                    WriteElementWithValue(writer, "PSC_STAMINA_RECHARGE", txtStaminaRecharge.Text);
                    WriteElementWithValue(writer, "PSC_F_52", txtF52.Text);
                    WriteElementWithValue(writer, "PSC_F_56", txtF56.Text);
                    WriteElementWithValue(writer, "PSC_I_60", txtI60.Text);
                    WriteElementWithValue(writer, "PSC_BASIC_ATK_DEF", txtBasicAtkDef.Text);
                    WriteElementWithValue(writer, "PSC_BASIC_KI_DEF", txtBasicKiDef.Text);

                    WriteElementWithValue(writer, "PSC_STRIKE_ATK_DEF", txtStrikeAtkDef.Text);
                    WriteElementWithValue(writer, "PSC_SUPER_KI_DEF", txtSuperKiDef.Text);
                    WriteElementWithValue(writer, "PSC_GROUND_SPEED", txtGroundSpeed.Text);
                    WriteElementWithValue(writer, "PSC_AIR_SPEED", txtAirSpeed.Text);
                    WriteElementWithValue(writer, "PSC_BOOST_SPEED", txtBoostSpeed.Text);

                    WriteElementWithValue(writer, "PSC_DASH_SPEED", txtDashSpeed.Text);
                    WriteElementWithValue(writer, "PSC_F96", txtF96.Text);
                    WriteElementWithValue(writer, "PSC_REINFORCEMENT_SKILL", txtReinforcementSkill.Text);
                    WriteElementWithValue(writer, "PSC_F104", txtF104.Text);
                    WriteElementWithValue(writer, "PSC_REVIVAL_HP_AMOUNT", txtRevivalHpAmount.Text);

                    WriteElementWithValue(writer, "PSC_REVIVING_SPEED", txtRevivingSpeed.Text);
                    WriteElementWithValue(writer, "PSC_F_116", txtF116.Text);
                    WriteElementWithValue(writer, "PSC_F_120", txtF120.Text);
                    WriteElementWithValue(writer, "PSC_F_124", txtF124.Text);
                    WriteElementWithValue(writer, "PSC_F_128", txtF128.Text);

                    WriteElementWithValue(writer, "PSC_F_132", txtF132.Text);
                    WriteElementWithValue(writer, "PSC_F_136", txtF136.Text);
                    WriteElementWithValue(writer, "PSC_I_140", txtI140.Text);
                    WriteElementWithValue(writer, "PSC_F_144", txtF144.Text);
                    WriteElementWithValue(writer, "PSC_F_148", txtF148.Text);

                    WriteElementWithValue(writer, "PSC_F_152", txtF152.Text);
                    WriteElementWithValue(writer, "PSC_F_156", txtF156.Text);
                    WriteElementWithValue(writer, "PSC_F_160", txtF160.Text);
                    WriteElementWithValue(writer, "PSC_F_164", txtF164.Text);
                    WriteElementWithValue(writer, "PSC_Z_SOUL", txtZSoul.Text);

                    WriteElementWithValue(writer, "PSC_I_172", txtI172.Text);
                    WriteElementWithValue(writer, "PSC_I_176", txtI176.Text);
                    WriteElementWithValue(writer, "PSC_F_180", txtF180.Text);


                    WriteElementWithValue(writer, "MSG_CHARACTER_NAME", txtMSG1.Text);
                    WriteElementWithValue(writer, "MSG_COSTUME_NAME", txtMSG2.Text);

                    WriteElementWithValue(writer, "VOX_1", txtVOX1.Text);
                    WriteElementWithValue(writer, "VOX_2", txtVOX2.Text);

                    writer.WriteEndElement(); // Close XVMOD
                    writer.WriteEndDocument(); // Close the document
                }
                Directory.CreateDirectory("./XVCharaCreatorTemp/chara/");
                CopyDirectory(txtFolder.Text, @"./XVCharaCreatorTemp/chara/" + txtCharID.Text);
                Directory.CreateDirectory("./XVCharaCreatorTemp/ui/texture/CHARA01");
                File.Move(textBox1.Text, @"./XVCharaCreatorTemp/ui/texture/CHARA01/" + txtCharID.Text + "_000.DDS");
                Directory.CreateDirectory("./XVCharaCreatorTemp/JUNGLE");
                if (textBox2.Text.Length > 0)
                    CopyDirectory(textBox2.Text, @"./XVCharaCreatorTemp/JUNGLE");
                ZipFile.CreateFromDirectory(@"./XVCharaCreatorTemp/", sfd.FileName);
                if (File.Exists(xmlFilePath))
                    File.Delete(xmlFilePath);
                MessageBox.Show("Mod Created Successfully!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                Directory.Delete("./XVCharaCreatorTemp", true);
            }

        }
        private void WriteElementWithValue(XmlWriter writer, string elementName, string value)
        {
            if (!string.IsNullOrEmpty(value))  // Check if value is not empty or null
            {
                // Remove the surrounding quotes if present
                if (value.StartsWith("\"") && value.EndsWith("\""))
                {
                    value = value.Substring(1, value.Length - 2);  // Remove the surrounding quotes
                }

                writer.WriteStartElement(elementName);
                writer.WriteAttributeString("value", value);  // Add value as an attribute
                writer.WriteEndElement();
            }
            else
            {
                string tempVal = "\"\"";
                // Remove the surrounding quotes if present
                if (value.StartsWith("\"") && value.EndsWith("\""))
                {
                    value = tempVal.Substring(1, value.Length - 2);  // Remove the surrounding quotes
                }

                writer.WriteStartElement(elementName);
                writer.WriteAttributeString("value", value);  // Add value as an attribute
                writer.WriteEndElement();
            }
        }

        private void SetComboBoxValueSuper(XmlWriter xmlWriter, ComboBox comboBox, string elementName)
        {
            if (comboBox.SelectedIndex >= 0)
                WriteElementWithValue(xmlWriter, elementName, CS.Supers[comboBox.SelectedIndex].ID.ToString());
            else
                WriteElementWithValue(xmlWriter, elementName, "");
        }
        private void SetComboBoxValueUltimate(XmlWriter xmlWriter, ComboBox comboBox, string elementName)
        {
            if (comboBox.SelectedIndex >= 0)
                WriteElementWithValue(xmlWriter, elementName, CS.Ultimates[comboBox.SelectedIndex].ID.ToString());
            else
                WriteElementWithValue(xmlWriter, elementName, "");
        }
        private void SetComboBoxValueEvasive(XmlWriter xmlWriter, ComboBox comboBox, string elementName)
        {
            if (comboBox.SelectedIndex >= 0)
                WriteElementWithValue(xmlWriter, elementName, CS.Evasives[comboBox.SelectedIndex].ID.ToString());
            else
                WriteElementWithValue(xmlWriter, elementName, "");
        }

        private void btnGenID_Click(object sender, EventArgs e)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            Random random = new Random();
            char[] id = new char[3];
            string generatedID;

            for (int i = 0; i < 3; i++)
            {
                id[i] = chars[random.Next(chars.Length)];
            }

            generatedID = new string(id);


            txtCharID.Text = generatedID;
        }

        private void btnFolder_Click(object sender, EventArgs e)
        {
            if (txtCharID.Text.Length == 3)
            {
                FolderBrowserDialog fbd = new FolderBrowserDialog();
                fbd.Description = $"Select {txtCharID.Text} Folder";
                fbd.UseDescriptionForTitle = true;

                if (fbd.ShowDialog() == DialogResult.OK)
                {
                    string selectedPath = fbd.SelectedPath;
                    string selectedDirName = Path.GetFileName(selectedPath);

                    if (selectedDirName == txtCharID.Text && Directory.Exists(selectedPath))
                    {
                        txtFolder.Text = selectedPath;
                    }
                    else
                    {
                        MessageBox.Show($"Please select the folder matching {txtCharID.Text}.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            FolderBrowserDialog datadialog = new FolderBrowserDialog();
            datadialog.Description = "Select \"DB Xenoverse\" folder";

            if (Directory.Exists("D:\\SteamLibrary\\steamapps\\common\\DB Xenoverse"))
            {

                Properties.Settings.Default.data_path = "D:\\SteamLibrary\\steamapps\\common\\DB Xenoverse\\data";
                Properties.Settings.Default.Save();
            }

            if (Properties.Settings.Default.data_path.Length == 0)
            {
                if (datadialog.ShowDialog() == DialogResult.OK)
                {
                    Properties.Settings.Default.data_path = datadialog.SelectedPath + @"/data";
                    Properties.Settings.Default.Save();
                }
            }

            if (!Directory.Exists(Settings.Default.data_path))
                MessageBox.Show("Data folder not found, you must start XVReborn first", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

            if (Settings.Default.language == "uninitialized")
            {
                Form2 frm = new Form2();
                frm.ShowDialog();
            }

            CS.populateSkillData(Settings.Default.data_path + @"/msg", Settings.Default.data_path + @"/system/custom_skill.cus", Settings.Default.language);

            //populate skill lists
            foreach (skill sk in CS.Supers)
            {
                cbSuper1.Items.Add(sk.Name);
                cbSuper2.Items.Add(sk.Name);
                cbSuper3.Items.Add(sk.Name);
                cbSuper4.Items.Add(sk.Name);
            }
            foreach (skill sk in CS.Ultimates)
            {
                cbUltimate1.Items.Add(sk.Name);
                cbUltimate2.Items.Add(sk.Name);
            }
            foreach (skill sk in CS.Evasives)
            {
                cbEvasive.Items.Add(sk.Name);
            }
        }
        private void txtAuraID_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)  // Allow digits and Backspace
            {
                e.Handled = true;  // Mark the event as handled, preventing non-digit input
            }
        }

        private void txtVersion_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (!char.IsDigit(e.KeyChar) && e.KeyChar != (char)8)  // Allow digits and Backspace
            {
                e.Handled = true;  // Mark the event as handled, preventing non-digit input
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = $"Select .DDS file";
            ofd.Filter = "Direct Draw Surface files (*.DDS)|*.DDS";
            if (ofd.ShowDialog() == DialogResult.OK && File.Exists(ofd.FileName))
            {
                textBox1.Text = ofd.FileName;
            }
        }

        private void sToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }
        public static void CopyDirectory(string sourceDir, string destDir, bool recursive = true)
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
                Directory.CreateDirectory(Path.GetDirectoryName(destFile));
                File.Copy(file, destFile, true);
            }
        }
        private void openToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Apri il file XVMod (.xvmod)
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Open Mod File";
            ofd.Filter = "Xenoverse Mod Files (*.xvmod)|*.xvmod";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                string tempDir = "./XVCharaCreatorTemp2";
                if (Directory.Exists(tempDir))
                    Directory.Delete(tempDir, true);
                Directory.CreateDirectory(tempDir);
                ZipFile.ExtractToDirectory(ofd.FileName, tempDir);
                string xmlFilePath = Path.Combine(tempDir, "xvmod.xml");
                if (!File.Exists(xmlFilePath))
                {
                    MessageBox.Show("Errore: il file xvmod.xml non è stato trovato all'interno del file .xvmod", "Errore", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                if (Directory.Exists(tempDir + @"/JUNGLE"))
                    textBox2.Text = tempDir + @"/JUNGLE";


                XDocument xmlDoc = XDocument.Load(xmlFilePath);
                string modType = xmlDoc.Descendants("XVMOD").FirstOrDefault()?.Attribute("type")?.Value ?? "";
                if (modType != "ADDED_CHARACTER")
                {
                    throw new NotImplementedException($"Mod type {modType} not supported");
                }

                txtName.Text = xmlDoc.Descendants("MOD_NAME").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtAuthor.Text = xmlDoc.Descendants("MOD_AUTHOR").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtCharID.Text = xmlDoc.Descendants("CMS_BCS").FirstOrDefault()?.Attribute("value")?.Value ?? "";  //Too lazy to get it from filenames xD
                txtAuraID.Text = xmlDoc.Descendants("AUR_ID").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                cbAuraGlare.Checked = xmlDoc.Descendants("AUR_GLARE").FirstOrDefault()?.Attribute("value")?.Value == "1";

                txtBCS.Text = xmlDoc.Descendants("CMS_BCS").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                if (Directory.Exists(tempDir + @"/" + txtCharID.Text))
                    txtFolder.Text = tempDir + @"/" + txtCharID.Text;


                txtEAN.Text = xmlDoc.Descendants("CMS_EAN").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtFCEEAN.Text = xmlDoc.Descendants("CMS_FCE_EAN").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtCAMEAN.Text = xmlDoc.Descendants("CMS_CAM_EAN").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtBAC.Text = xmlDoc.Descendants("CMS_BAC").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtBCM.Text = xmlDoc.Descendants("CMS_BCM").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtBAI.Text = xmlDoc.Descendants("CMS_BAI").FirstOrDefault()?.Attribute("value")?.Value ?? "";

                txtCSO1.Text = xmlDoc.Descendants("CSO_1").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtCSO2.Text = xmlDoc.Descendants("CSO_2").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtCSO3.Text = xmlDoc.Descendants("CSO_3").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtCSO4.Text = xmlDoc.Descendants("CSO_4").FirstOrDefault()?.Attribute("value")?.Value ?? "";


                SetComboBoxValueFromXMLSuper(xmlDoc, cbSuper1, "CUS_SUPER_1");
                SetComboBoxValueFromXMLSuper(xmlDoc, cbSuper2, "CUS_SUPER_2");
                SetComboBoxValueFromXMLSuper(xmlDoc, cbSuper3, "CUS_SUPER_3");
                SetComboBoxValueFromXMLSuper(xmlDoc, cbSuper4, "CUS_SUPER_4");
                SetComboBoxValueFromXMLUltimate(xmlDoc, cbUltimate1, "CUS_ULTIMATE_1");
                SetComboBoxValueFromXMLUltimate(xmlDoc, cbUltimate2, "CUS_ULTIMATE_2");
                SetComboBoxValueFromXMLEvasive(xmlDoc, cbEvasive, "CUS_EVASIVE");

                txtCostume.Text = xmlDoc.Descendants("PSC_COSTUME").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtPreset.Text = xmlDoc.Descendants("PSC_PRESET").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtCameraPosition.Text = xmlDoc.Descendants("PSC_CAMERA_POSITION").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtHealth.Text = xmlDoc.Descendants("PSC_HEALTH").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtI12.Text = xmlDoc.Descendants("PSC_I_12").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtF20.Text = xmlDoc.Descendants("PSC_F_20").FirstOrDefault()?.Attribute("value")?.Value ?? "";

                txtKi.Text = xmlDoc.Descendants("PSC_KI").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtKiRecharge.Text = xmlDoc.Descendants("PSC_KI_RECHARGE").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtI32.Text = xmlDoc.Descendants("PSC_I_32").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtI36.Text = xmlDoc.Descendants("PSC_I_36").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtI40.Text = xmlDoc.Descendants("PSC_I_40").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtStamina.Text = xmlDoc.Descendants("PSC_STAMINA").FirstOrDefault()?.Attribute("value")?.Value ?? "";

                txtStaminaRecharge.Text = xmlDoc.Descendants("PSC_STAMINA_RECHARGE").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtF52.Text = xmlDoc.Descendants("PSC_F_52").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtF56.Text = xmlDoc.Descendants("PSC_F_56").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtI60.Text = xmlDoc.Descendants("PSC_I_60").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtBasicAtkDef.Text = xmlDoc.Descendants("PSC_BASIC_ATK_DEFENSE").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtBasicKiDef.Text = xmlDoc.Descendants("PSC_BASIC_KI_DEFENSE").FirstOrDefault()?.Attribute("value")?.Value ?? "";

                txtStrikeAtkDef.Text = xmlDoc.Descendants("PSC_STRIKE_ATK_DEFENSE").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtSuperKiDef.Text = xmlDoc.Descendants("PSC_SUPER_KI_DEFENSE").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtGroundSpeed.Text = xmlDoc.Descendants("PSC_GROUND_SPEED").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtAirSpeed.Text = xmlDoc.Descendants("PSC_AIR_SPEED").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtBoostSpeed.Text = xmlDoc.Descendants("PSC_BOOST_SPEED").FirstOrDefault()?.Attribute("value")?.Value ?? "";

                txtDashSpeed.Text = xmlDoc.Descendants("PSC_DASH_SPEED").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtF96.Text = xmlDoc.Descendants("PSC_F_96").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtReinforcementSkill.Text = xmlDoc.Descendants("PSC_REINFORCEMENT_SKILL_DURATION").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtF104.Text = xmlDoc.Descendants("PSC_F_104").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtRevivalHpAmount.Text = xmlDoc.Descendants("PSC_REVIVAL_HP_AMOUNT").FirstOrDefault()?.Attribute("value")?.Value ?? "";

                txtRevivingSpeed.Text = xmlDoc.Descendants("PSC_REVIVING_SPEED").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtF116.Text = xmlDoc.Descendants("PSC_F_116").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtF120.Text = xmlDoc.Descendants("PSC_F_120").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtF124.Text = xmlDoc.Descendants("PSC_F_124").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtF128.Text = xmlDoc.Descendants("PSC_F_128").FirstOrDefault()?.Attribute("value")?.Value ?? "";

                txtF132.Text = xmlDoc.Descendants("PSC_F_132").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtF136.Text = xmlDoc.Descendants("PSC_F_136").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtI140.Text = xmlDoc.Descendants("PSC_I_140").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtF144.Text = xmlDoc.Descendants("PSC_F_144").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtF148.Text = xmlDoc.Descendants("PSC_F_148").FirstOrDefault()?.Attribute("value")?.Value ?? "";

                txtF152.Text = xmlDoc.Descendants("PSC_F_152").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtF156.Text = xmlDoc.Descendants("PSC_F_156").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtF160.Text = xmlDoc.Descendants("PSC_F_160").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtF164.Text = xmlDoc.Descendants("PSC_F_164").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtZSoul.Text = xmlDoc.Descendants("PSC_Z_SOUL").FirstOrDefault()?.Attribute("value")?.Value ?? "";

                txtI172.Text = xmlDoc.Descendants("PSC_I_172").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtI176.Text = xmlDoc.Descendants("PSC_I_176").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtF180.Text = xmlDoc.Descendants("PSC_F_180").FirstOrDefault()?.Attribute("value")?.Value ?? "";

                txtMSG1.Text = xmlDoc.Descendants("MSG_CHARACTER_NAME").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtMSG2.Text = xmlDoc.Descendants("MSG_COSTUME_NAME").FirstOrDefault()?.Attribute("value")?.Value ?? "";

                txtVOX1.Text = xmlDoc.Descendants("VOX_1").FirstOrDefault()?.Attribute("value")?.Value ?? "";
                txtVOX2.Text = xmlDoc.Descendants("VOX_2").FirstOrDefault()?.Attribute("value")?.Value ?? "";

                // Carica la directory del personaggio
                string charFolderPath = Path.Combine(tempDir, "chara", txtCharID.Text);
                if (Directory.Exists(charFolderPath))
                {
                    txtFolder.Text = charFolderPath;
                }

                // Carica l'immagine DDS
                string ddsFilePath = Path.Combine(tempDir, "ui", "texture", "CHARA01", txtCharID.Text + "_000.DDS");
                if (File.Exists(ddsFilePath))
                {
                    textBox1.Text = ddsFilePath;
                }
            }


        }
    
        private void SetComboBoxValueFromXMLSuper(XDocument xmlDoc, ComboBox comboBox, string elementName)
        {
            var elementValue = xmlDoc.Descendants(elementName).FirstOrDefault()?.Attribute("value")?.Value;

            if (!string.IsNullOrEmpty(elementValue))
            {
                int skillID = int.Parse(elementValue);

                for (int i = 0; i < CS.Supers.Count(); i++)
                {
                    if (CS.Supers[i].ID == skillID)
                    {
                        comboBox.SelectedItem = CS.Supers[i].Name;
                        break;
                    }
                }
            }
            else
            {
                comboBox.SelectedIndex = -1;
            }
        }

        private void SetComboBoxValueFromXMLUltimate(XDocument xmlDoc, ComboBox comboBox, string elementName)
        {
            var elementValue = xmlDoc.Descendants(elementName).FirstOrDefault()?.Attribute("value")?.Value;

            if (!string.IsNullOrEmpty(elementValue))
            {
                int skillID = int.Parse(elementValue);

                for (int i = 0; i < CS.Ultimates.Count(); i++)
                {
                    if (CS.Ultimates[i].ID == skillID)
                    {
                        comboBox.SelectedItem = CS.Ultimates[i].Name;
                        break;
                    }
                }
            }
            else
            {
                comboBox.SelectedIndex = -1;
            }
        }


        private void SetComboBoxValueFromXMLEvasive(XDocument xmlDoc, ComboBox comboBox, string elementName)
        {
            var elementValue = xmlDoc.Descendants(elementName).FirstOrDefault()?.Attribute("value")?.Value;

            if (!string.IsNullOrEmpty(elementValue))
            {
                int skillID = int.Parse(elementValue);

                for (int i = 0; i < CS.Evasives.Count(); i++)
                {
                    if (CS.Evasives[i].ID == skillID)
                    {
                        comboBox.SelectedItem = CS.Evasives[i].Name;
                        break;
                    }
                }
            }
            else
            {
                comboBox.SelectedIndex = -1;
            }
        }

        private void btnAdditionalFiles_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();
            fbd.Description = "Select data folder";
            if (fbd.ShowDialog() == DialogResult.OK)
                textBox2.Text = fbd.SelectedPath;
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (Directory.Exists("./XVCharaCreatorTemp"))
                Directory.Delete("./XVCharaCreatorTemp", true);
            if (Directory.Exists("./XVCharaCreatorTemp2"))
                Directory.Delete("./XVCharaCreatorTemp2", true);
        }
    }
}