using FreeImageAPI;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using XVReborn.Properties;
using XVReborn.XV;
using static System.Net.Mime.MediaTypeNames;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace XVReborn
{

    public partial class Form1 : Form
    {
        string language = "";

        public Form1()
        {
            InitializeComponent();

            ContextMenuStrip contextMenu = new ContextMenuStrip();

            ToolStripMenuItem uninstallMenuItem = new ToolStripMenuItem("Uninstall Mod");

            contextMenu.Items.Add(uninstallMenuItem);

            lvMods.ContextMenuStrip = contextMenu;

            uninstallMenuItem.Click += (sender, e) =>
            {
                if (lvMods.SelectedItems.Count > 0)
                {
                    ListViewItem selectedItem = lvMods.SelectedItems[0];

                    uninstallMod(sender, e);
                }
                else
                {
                    MessageBox.Show("No mod selected for uninstallation.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            };

            lvMods.MouseUp += (sender, e) =>
            {
                if (e.Button == MouseButtons.Right)
                {
                    ListViewItem itemUnderCursor = lvMods.GetItemAt(e.X, e.Y);

                    if (itemUnderCursor != null)
                    {
                        lvMods.SelectedItems.Clear();
                        itemUnderCursor.Selected = true; 
                        contextMenu.Show(lvMods, e.Location);
                    }
                }
            };

        }

        public void Form1_Load(object sender, EventArgs e)
        {
            

            if (Settings.Default.language.Length == 0)
            {
                Form2 frm = new Form2();
                frm.ShowDialog();
                language = Settings.Default.language;
            }
            else
            {
                language = Settings.Default.language;
            }

            if (Properties.Settings.Default.datafolder.Length == 0)
            {
                if (Directory.Exists("D:\\SteamLibrary\\steamapps\\common\\DB Xenoverse\\"))
                {
                    string dataPath = "D:\\SteamLibrary\\steamapps\\common\\DB Xenoverse\\data";
                    Directory.CreateDirectory(dataPath);
                    Settings.Default.datafolder = dataPath;
                    Settings.Default.Save();
                }
                else
                {
                    OpenFileDialog gameExe = new OpenFileDialog();
                    gameExe.Filter = "DBXV.exe|DBXV.exe";


                    if (gameExe.ShowDialog() == DialogResult.OK)
                    {
                        string dataPath = Path.GetDirectoryName(gameExe.FileName) + @"/data";
                        Directory.CreateDirectory(dataPath);
                        Settings.Default.datafolder = dataPath;
                        Settings.Default.Save();
                    }
                    else
                    {
                        this.Close();
                    }
                }
            }
            else
            {
                if (!Directory.Exists(Properties.Settings.Default.datafolder))
                    MessageBox.Show("Data Folder not Found, Please Clear Installation", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            if (!File.Exists(Properties.Settings.Default.datafolder + @"\ui\texture\embpack.exe"))
            {
                var myAssembly = Assembly.GetExecutingAssembly();
                var myStream = myAssembly.GetManifestResourceStream("XVReborn.ZipFile_Blobs.embpack.zip");
                ZipArchive archive = new ZipArchive(myStream);
                archive.ExtractToDirectory(Path.Combine(Settings.Default.datafolder + @"\ui\texture"));
            }

            if (!Directory.Exists(Properties.Settings.Default.datafolder + @"\ui\texture\CHARA01"))
            {
                if (!Directory.Exists(Properties.Settings.Default.datafolder + @"\ui\texture"))
                {
                    Directory.CreateDirectory(Properties.Settings.Default.datafolder + @"\ui\texture");
                }

                var myAssembly = Assembly.GetExecutingAssembly();
                var myStream = myAssembly.GetManifestResourceStream("XVReborn.ZipFile_Blobs.CHARA01.zip");
                ZipArchive archive = new ZipArchive(myStream);

                if (!File.Exists(Path.Combine(Settings.Default.datafolder + @"\ui\texture\CHARA01.emb")))
                    archive.ExtractToDirectory(Path.Combine(Settings.Default.datafolder + @"\ui\texture"));

                Process p = new Process();
                ProcessStartInfo info = new ProcessStartInfo();
                info.FileName = "cmd.exe";
                info.CreateNoWindow = true;
                info.WindowStyle = ProcessWindowStyle.Hidden;
                info.RedirectStandardInput = true;
                info.UseShellExecute = false;

                p.StartInfo = info;
                p.Start();

                using (StreamWriter sw = p.StandardInput)
                {
                    if (sw.BaseStream.CanWrite)
                    {
                        sw.WriteLine("cd " + Properties.Settings.Default.datafolder + @"\ui\texture");
                        sw.WriteLine(@"embpack.exe CHARA01.emb");
                    }
                }
            }

            if (!Directory.Exists(Properties.Settings.Default.datafolder + @"\system"))
            {
                Directory.CreateDirectory(Properties.Settings.Default.datafolder + @"\system");

                var myAssembly = Assembly.GetExecutingAssembly();

                var myStream = myAssembly.GetManifestResourceStream("XVReborn.ZipFile_Blobs.char_model_spec.zip");
                var myStream2 = myAssembly.GetManifestResourceStream("XVReborn.ZipFile_Blobs.chara_sound.zip");
                var myStream3 = myAssembly.GetManifestResourceStream("XVReborn.ZipFile_Blobs.parameter_spec_char.zip");
                var myStream4 = myAssembly.GetManifestResourceStream("XVReborn.ZipFile_Blobs.aura_setting.zip");
                var myStream5 = myAssembly.GetManifestResourceStream("XVReborn.ZipFile_Blobs.custom_skill.zip");
                var myStream6 = myAssembly.GetManifestResourceStream("XVReborn.ZipFile_Blobs.XMLSerializer.zip");
                var myStream7 = myAssembly.GetManifestResourceStream("XVReborn.ZipFile_Blobs.item.zip");

                ZipArchive archive = new ZipArchive(myStream);
                ZipArchive archive2 = new ZipArchive(myStream2);
                ZipArchive archive3 = new ZipArchive(myStream3);
                ZipArchive archive4 = new ZipArchive(myStream4);
                ZipArchive archive5 = new ZipArchive(myStream5);
                ZipArchive archive6 = new ZipArchive(myStream6);
                ZipArchive archive7 = new ZipArchive(myStream7);

                archive.ExtractToDirectory(Path.Combine(Settings.Default.datafolder + @"\system"));
                archive2.ExtractToDirectory(Path.Combine(Settings.Default.datafolder + @"\system"));
                archive3.ExtractToDirectory(Path.Combine(Settings.Default.datafolder + @"\system"));
                archive4.ExtractToDirectory(Path.Combine(Settings.Default.datafolder + @"\system"));
                archive5.ExtractToDirectory(Path.Combine(Settings.Default.datafolder + @"\system"));
                archive6.ExtractToDirectory(Path.Combine(Settings.Default.datafolder + @"\system"));
                archive7.ExtractToDirectory(Path.Combine(Settings.Default.datafolder + @"\system"));
            }

            if (!Directory.Exists(Properties.Settings.Default.datafolder + @"\ui\iggy"))
            {
                Directory.CreateDirectory(Properties.Settings.Default.datafolder + @"\system");

                var myAssembly = Assembly.GetExecutingAssembly();

                var myStream = myAssembly.GetManifestResourceStream("XVReborn.ZipFile_Blobs.CHARASELE.zip");
                var myStream3 = myAssembly.GetManifestResourceStream("XVReborn.ZipFile_Blobs.XVP_SLOTS.zip");

                ZipArchive archive = new ZipArchive(myStream);
                ZipArchive archive3 = new ZipArchive(myStream3);

                archive.ExtractToDirectory(Path.Combine(Settings.Default.datafolder + @"\ui\iggy"));

                if (!File.Exists(Settings.Default.datafolder + @"\ui\iggy\XVP_SLOTS.xs"))
                    archive3.ExtractToDirectory(Path.Combine(Settings.Default.datafolder));
            }

            if (!Directory.Exists(Properties.Settings.Default.datafolder + @"\msg"))
            {
                var myAssembly = Assembly.GetExecutingAssembly();
                var myStream = myAssembly.GetManifestResourceStream("XVReborn.ZipFile_Blobs.msg.zip");
                ZipArchive archive = new ZipArchive(myStream);
                archive.ExtractToDirectory(Path.Combine(Settings.Default.datafolder + @"\msg"));
            }

            if (Properties.Settings.Default.modlist.Contains("System.Object"))
            {
                Properties.Settings.Default.modlist.Clear();
            }

            if (Properties.Settings.Default.addonmodlist.Contains("System.Object"))
            {
                Properties.Settings.Default.addonmodlist.Clear();
            }

            loadLvItems();
            Clean();
        }
        private void Clean()
        {
            if (File.Exists(Properties.Settings.Default.datafolder + "//modinfo.xml"))
            {
                File.Delete(Properties.Settings.Default.datafolder + "//modinfo.xml");
            }

            if (Directory.Exists(Properties.Settings.Default.datafolder + "//temp"))
            {
                Directory.Delete(Properties.Settings.Default.datafolder + "//temp", true);
            }

            if (File.Exists(Properties.Settings.Default.datafolder + @"\system\aura_setting.aur.xml"))
            {
                File.Delete(Properties.Settings.Default.datafolder + @"\system\aura_setting.aur.xml");
            }

            if (File.Exists(Properties.Settings.Default.datafolder + @"\system\aura_setting.aur.xml.bak"))
            {
                File.Delete(Properties.Settings.Default.datafolder + @"\system\aura_setting.aur.xml.bak");
            }

            if (File.Exists(Properties.Settings.Default.datafolder + @"\system\custom_skill.cus.xml"))
            {
                File.Delete(Properties.Settings.Default.datafolder + @"\system\custom_skill.cus.xml");
            }

            if (File.Exists(Properties.Settings.Default.datafolder + @"\system\custom_skill.cus.xml.bak"))
            {
                File.Delete(Properties.Settings.Default.datafolder + @"\system\custom_skill.cus.xml.bak");
            }

            if (File.Exists(Properties.Settings.Default.datafolder + @"\system\char_model_spec.cms.xml"))
            {
                File.Delete(Properties.Settings.Default.datafolder + @"\system\char_model_spec.cms.xml");
            }

            if (File.Exists(Properties.Settings.Default.datafolder + @"\system\char_model_spec.cms.xml.bak"))
            {
                File.Delete(Properties.Settings.Default.datafolder + @"\system\char_model_spec.cms.xml.bak");
            }

            if (File.Exists(Properties.Settings.Default.datafolder + @"\system\parameter_spec_char.psc.xml"))
            {
                File.Delete(Properties.Settings.Default.datafolder + @"\system\parameter_spec_char.psc.xml");
            }

            if (File.Exists(Properties.Settings.Default.datafolder + @"\system\parameter_spec_char.psc.xml.bak"))
            {
                File.Delete(Properties.Settings.Default.datafolder + @"\system\parameter_spec_char.psc.xml.bak");
            }

            if (File.Exists(Properties.Settings.Default.datafolder + @"\system\chara_sound.cso.xml"))
            {
                File.Delete(Properties.Settings.Default.datafolder + @"\system\chara_sound.cso.xml");
            }

            if (File.Exists(Properties.Settings.Default.datafolder + @"\system\chara_sound.cso.xml.bak"))
            {
                File.Delete(Properties.Settings.Default.datafolder + @"\system\chara_sound.cso.xml.bak");
            }
            /*
            if (File.Exists(Properties.Settings.Default.datafolder + @"\quest\TMQ\tmq_data.qxd.bak"))
            {
                File.Delete(Properties.Settings.Default.datafolder + @"\quest\TMQ\tmq_data.qxd.bak");
            }
            */

            if (Directory.Exists("./XVRebornTemp"))
                Directory.Delete("./XVRebornTemp", true);
            if (File.Exists(Settings.Default.datafolder + "\\x2m.xml"))
                File.Delete(Settings.Default.datafolder + "\\x2m.xml");

            RemoveEmptyDirectories(Settings.Default.datafolder);
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            Clean();
        }



        private void clearInstallationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to clear installation?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
            {
                if (Directory.Exists(Properties.Settings.Default.datafolder))
                    Directory.Delete(Properties.Settings.Default.datafolder, true);

                if (Directory.Exists("./XVRebornTemp"))
                    Directory.Delete("./XVRebornTemp", true);

                Properties.Settings.Default.Reset();
                MessageBox.Show("Installation cleared, XVReborn will now close", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                this.Close();
            }
            else
            {
                return;
            }
        }

        private void saveLvItems()
        {
            Properties.Settings.Default.modlist = new StringCollection();
            Properties.Settings.Default.modlist.AddRange((from i in this.lvMods.Items.Cast<ListViewItem>()
                                                          select string.Join("|", from si in i.SubItems.Cast<ListViewItem.ListViewSubItem>()
                                                                                  select si.Text)).ToArray());
            Properties.Settings.Default.Save();
            label1.Text = "Installed Mods: " + lvMods.Items.Count.ToString();
        }

        private void loadLvItems()
        {
            if (Properties.Settings.Default.modlist == null)
            {
                Properties.Settings.Default.modlist = new StringCollection();
            }

            this.lvMods.Items.AddRange((from i in Properties.Settings.Default.modlist.Cast<string>()
                                        select new ListViewItem(i.Split('|'))).ToArray());

            label1.Text = "Installed Mods: " + lvMods.Items.Count.ToString();
        }
        private List<string> EnumerateFiles(string directory)
        {
            List<string> files = new List<string>();
            foreach (string file in Directory.GetFiles(directory, "*", SearchOption.AllDirectories))
            {
                files.Add(file);
            }
            return files;
        }

        private void installmod(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Install Mod";
            ofd.Filter = "Xenoverse Mod Files (*.xvmod)|*.xvmod";
            ofd.Multiselect = true;
            ofd.CheckFileExists = true;

            string modtype = "";
            string modname = "";
            string modauthor = "";
            string modversion = "";
            int AUR_ID = 0;
            int AUR_GLARE = 0;
            string CMS_BCS = "";
            string CMS_EAN = "";
            string CMS_FCE_EAN = "";
            string CMS_CAM_EAN = "";
            string CMS_BAC = "";
            string CMS_BCM = "";
            string CMS_BAI = "";
            string CSO_1 = "";
            string CSO_2 = "";
            string CSO_3 = "";
            string CSO_4 = "";
            string CUS_SUPER_1 = "";
            string CUS_SUPER_2 = "";
            string CUS_SUPER_3 = "";
            string CUS_SUPER_4 = "";
            string CUS_ULTIMATE_1 = "";
            string CUS_ULTIMATE_2 = "";
            string CUS_EVASIVE = "";
            string PSC_COSTUME = "";
            string PSC_PRESET = "";
            string PSC_CAMERA_POS = "";
            string PSC_HEALTH = "";
            string PSC_I_12 = "";
            string PSC_F_20 = "";
            string PSC_KI = "";
            string PSC_KI_RECHARGE = "";
            string PSC_I_32 = "";
            string PSC_I_36 = "";
            string PSC_I_40 = "";
            string PSC_STAMINA = "";
            string PSC_STAMINA_RECHARGE = "";
            string PSC_F_52 = "";
            string PSC_F_56 = "";
            string PSC_I_60 = "";
            string PSC_BASIC_ATK_DEF = "";
            string PSC_BASIC_KI_DEF = "";
            string PSC_STRIKE_ATK_DEF = "";
            string PSC_SUPER_KI_DEF = "";
            string PSC_GROUND_SPEED = "";
            string PSC_AIR_SPEED = "";
            string PSC_BOOST_SPEED = "";
            string PSC_DASH_SPEED = "";
            string PSC_F_96 = "";
            string PSC_REINFORCEMENT_SKILL = "";
            string PSC_F_104 = "";
            string PSC_REVIVAL_HP_AMOUNT = "";
            string PSC_REVIVAL_SPEED = "";
            string PSC_F_116 = "";
            string PSC_F_120 = "";
            string PSC_F_124 = "";
            string PSC_F_128 = "";
            string PSC_F_132 = "";
            string PSC_F_136 = "";
            string PSC_I_140 = "";
            string PSC_F_144 = "";
            string PSC_F_148 = "";
            string PSC_F_152 = "";
            string PSC_F_156 = "";
            string PSC_F_160 = "";
            string PSC_F_164 = "";
            string PSC_Z_SOUL = "";
            string PSC_I_172 = "";
            string PSC_I_176 = "";
            string PSC_F_180 = "";
            string MSG_CHARACTER_NAME = "";
            string MSG_COSTUME_NAME = "";
            short VOX_1 = -1;
            short VOX_2 = -1;
            string SKILL_ShortName = "";
            string SKILL_ID1 = "";
            string SKILL_ID2 = "";
            string SKILL_I_04 = "";
            string SKILL_Race_Lock = "";
            string SKILL_Type = "";
            string SKILL_FilesLoaded = "";
            string SKILL_PartSet = "";
            string SKILL_I_18 = "";
            string SKILL_EAN = "";
            string SKILL_CAM_EAN = "";
            string SKILL_EEPK = "";
            string SKILL_ACB_SE = "";
            string SKILL_ACB_VOX = "";
            string SKILL_AFTER_BAC = "";
            string SKILL_AFTER_BCM = "";
            string SKILL_I_48 = "";
            string SKILL_I_50 = "";
            string SKILL_I_52 = "";
            string SKILL_I_54 = "";
            string SKILL_PUP = "";
            string SKILL_CUS_Aura = "";
            string SKILL_TransformCharaSwap = "";
            string SKILL_Skillset_Change = "";
            string SKILL_Num_Of_Transforms = "";
            string SKILL_I_66 = "";

            // The existing values
            string SKILL_TYPE = "";
            string MSG_SKILL_NAME = "";
            string MSG_SKILL_DESC = "";

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in ofd.FileNames)
                {
                    string xmlfile = "./XVRebornTemp" + @"/xvmod.xml";
                    int CharID = 108 + Settings.Default.modlist.Count;

                    if (File.Exists(Settings.Default.datafolder + @"/xvmod.xml"))
                        File.Delete(Settings.Default.datafolder + @"/xvmod.xml");
                    if (File.Exists(xmlfile))
                        File.Delete(xmlfile);
                    ZipFile.ExtractToDirectory(file, "./XVRebornTemp");

                    if (!File.Exists(xmlfile))
                        MessageBox.Show("xvmod.xml file not found.",
                        "Invalid mod file", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    string xmlData = File.ReadAllText(xmlfile);

                    using (XmlReader reader = XmlReader.Create(new StringReader(xmlData)))
                    {
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {

                                if (reader.Name == "XVMOD")
                                {
                                    if (reader.GetAttribute("type").Length == 0)
                                    {
                                        MessageBox.Show("Invalid xmlreader attribute.",
                                        "Invalid mod file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }

                                    modtype = reader.GetAttribute("type");

                                }

                                if (reader.Name == "XVMOD")
                                {
                                    if (reader.GetAttribute("type").Length == 0)
                                    {
                                        MessageBox.Show("Invalid xmlreader attribute.", "Invalid mod file", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                    }
                                    modtype = reader.GetAttribute("type");
                                }
                                if (reader.Name == "MOD_NAME")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        modname = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "MOD_AUTHOR")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        modauthor = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "MOD_VERSION")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        modversion = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "AUR_ID")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        bool parseSuccess = Int32.TryParse(reader.GetAttribute("value").Trim(), out AUR_ID);
                                        if (!parseSuccess)
                                        {
                                            // Gestisci il caso in cui la conversione non riesce, ad esempio, fornisci un valore predefinito o mostra un messaggio di errore.
                                            MessageBox.Show("AUR_ID value not recognized", "Error", MessageBoxButtons.OK);
                                            return;
                                        }
                                    }
                                }
                                if (reader.Name == "AUR_GLARE")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        bool parseSuccess = Int32.TryParse(reader.GetAttribute("value").Trim(), out AUR_GLARE);
                                        if (!parseSuccess)
                                        {
                                            // Gestisci il caso in cui la conversione non riesce, ad esempio, fornisci un valore predefinito o mostra un messaggio di errore.
                                            MessageBox.Show("AUR_GLARE value not recognized", "Error", MessageBoxButtons.OK);
                                            return;
                                        }
                                    }
                                }
                                if (reader.Name == "CMS_BCS")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CMS_BCS = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CMS_EAN")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CMS_EAN = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CMS_FCE_EAN")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CMS_FCE_EAN = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CMS_CAM_EAN")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CMS_CAM_EAN = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CMS_BAC")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CMS_BAC = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CMS_BCM")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CMS_BCM = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CMS_BAI")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CMS_BAI = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CSO_1")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CSO_1 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CSO_2")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CSO_2 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CSO_3")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CSO_3 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CSO_4")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CSO_4 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CUS_SUPER_1")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CUS_SUPER_1 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CUS_SUPER_2")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CUS_SUPER_2 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CUS_SUPER_3")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CUS_SUPER_3 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CUS_SUPER_4")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CUS_SUPER_4 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CUS_ULTIMATE_1")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CUS_ULTIMATE_1 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CUS_ULTIMATE_2")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CUS_ULTIMATE_2 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "CUS_EVASIVE")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        CUS_EVASIVE = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "PSC_COSTUME")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        PSC_COSTUME = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "PSC_PRESET")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        PSC_PRESET = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "PSC_CAMERA_POS")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        PSC_CAMERA_POS = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "PSC_HEALTH")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        PSC_HEALTH = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "PSC_I_12")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        PSC_I_12 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "PSC_F_20")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        PSC_F_20 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "PSC_KI")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        PSC_KI = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "PSC_KI_RECHARGE")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        PSC_KI_RECHARGE = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "PSC_I_32")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        PSC_I_32 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "PSC_I_36")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        PSC_I_36 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "PSC_I_40")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        PSC_I_40 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "PSC_STAMINA")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        PSC_STAMINA = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "PSC_STAMINA_RECHARGE")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        PSC_STAMINA_RECHARGE = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "PSC_F_52")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        PSC_F_52 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "PSC_F_56")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        PSC_F_56 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "PSC_I_60")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        PSC_I_60 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "PSC_BASIC_ATK_DEF")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        PSC_BASIC_ATK_DEF = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "PSC_BASIC_KI_DEF")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        PSC_BASIC_KI_DEF = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "PSC_STRIKE_ATK_DEF")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        PSC_STRIKE_ATK_DEF = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "PSC_SUPER_KI_DEF")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        PSC_SUPER_KI_DEF = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "PSC_GROUND_SPEED")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        PSC_GROUND_SPEED = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "PSC_AIR_SPEED")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        PSC_AIR_SPEED = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "PSC_BOOST_SPEED")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        PSC_BOOST_SPEED = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "PSC_DASH_SPEED")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        PSC_DASH_SPEED = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "PSC_F_96")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        PSC_F_96 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "PSC_REINFORCEMENT_SKILL")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        PSC_REINFORCEMENT_SKILL = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "PSC_F_104")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        PSC_F_104 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "PSC_REVIVAL_HP_AMOUNT")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        PSC_REVIVAL_HP_AMOUNT = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "PSC_REVIVAL_SPEED")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        PSC_REVIVAL_SPEED = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "PSC_F_116")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        PSC_F_116 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "PSC_F_120")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        PSC_F_120 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "PSC_F_124")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        PSC_F_124 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "PSC_F_128")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        PSC_F_128 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "PSC_F_132")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        PSC_F_132 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "PSC_F_136")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        PSC_F_136 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "PSC_I_140")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        PSC_I_140 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "PSC_F_144")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        PSC_F_144 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "PSC_F_148")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        PSC_F_148 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "PSC_F_152")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        PSC_F_152 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "PSC_F_156")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        PSC_F_156 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "PSC_F_160")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        PSC_F_160 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "PSC_F_164")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        PSC_F_164 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "PSC_Z_SOUL")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        PSC_Z_SOUL = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "PSC_I_172")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        PSC_I_172 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "PSC_I_176")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        PSC_I_176 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "PSC_F_180")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        PSC_F_180 = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "MSG_CHARACTER_NAME")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        MSG_CHARACTER_NAME = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "MSG_COSTUME_NAME")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        MSG_COSTUME_NAME = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "VOX_1")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        bool parseSuccess = Int16.TryParse(reader.GetAttribute("value").Trim(), out VOX_1);
                                        if (!parseSuccess)
                                        {
                                            MessageBox.Show("VOX_1 value not recognized", "Error", MessageBoxButtons.OK);
                                            return;
                                        }
                                    }
                                }
                                if (reader.Name == "VOX_2")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        bool parseSuccess = Int16.TryParse(reader.GetAttribute("value").Trim(), out VOX_2);
                                        if (!parseSuccess)
                                        {
                                            MessageBox.Show("VOX_2 value not recognized", "Error", MessageBoxButtons.OK);
                                            return;
                                        }
                                    }
                                }

                                if(reader.Name == "MSG_SKILL_NAME")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        MSG_SKILL_NAME = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "MSG_SKILL_DESC")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        MSG_SKILL_DESC = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "SKILL_TYPE")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_TYPE = reader.GetAttribute("value").Trim();
                                    }
                                }
                                if (reader.Name == "ShortName")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_ShortName = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "ID1")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_ID1 = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "ID2")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_ID2 = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "I_04")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_I_04 = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "Race_Lock")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_Race_Lock = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "Type")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_Type = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "FilesLoaded")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_FilesLoaded = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "PartSet")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_PartSet = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "I_18")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_I_18 = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "EAN")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_EAN = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "CAM_EAN")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_CAM_EAN = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "EEPK")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_EEPK = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "ACB_SE")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_ACB_SE = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "ACB_VOX")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_ACB_VOX = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "AFTER_BAC")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_AFTER_BAC = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "AFTER_BCM")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_AFTER_BCM = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "I_48")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_I_48 = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "I_50")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_I_50 = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "I_52")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_I_52 = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "I_54")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_I_54 = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "PUP")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_PUP = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "CUS_Aura")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_CUS_Aura = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "TransformCharaSwap")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_TransformCharaSwap = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "Skillset_Change")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_Skillset_Change = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "Num_Of_Transforms")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_Num_Of_Transforms = reader.GetAttribute("value").Trim();
                                    }
                                }

                                if (reader.Name == "I_66")
                                {
                                    if (reader.HasAttributes)
                                    {
                                        SKILL_I_66 = reader.GetAttribute("value").Trim();
                                    }
                                }
                            }
                        }
                        if (File.Exists(xmlfile))
                            File.Delete(xmlfile);

                    }
                    if (modtype == "REPLACER")
                    {
                        List<string> installedFiles = EnumerateFiles("./XVRebornTemp");
                        List<string> final = new List<string>();

                        foreach (string installedFile in installedFiles)
                        {
                            // Correctly replace the path and add it to the list
                            string modifiedFile = installedFile.Replace("./XVRebornTemp", Properties.Settings.Default.datafolder);
                            final.Add(modifiedFile);
                        }

                        // Ensure the target directory exists
                        string installedDir = Path.Combine(Settings.Default.datafolder, "installed");
                        if (!Directory.Exists(installedDir))
                        {
                            Directory.CreateDirectory(installedDir);
                        }

                        // Write the modified paths to a file
                        string filePath = Path.Combine(installedDir, $"{modtype}_{modname}.txt");
                        File.WriteAllLines(filePath, final);

                        MergeDirectoriesWithConfirmation("./XVRebornTemp", Settings.Default.datafolder);

                        Clean();


                        string[] row = { modname, modauthor, "Replacer" };
                        ListViewItem lvi = new ListViewItem(row);
                        lvMods.Items.Add(lvi);
                        saveLvItems();
                    }
                    else if (modtype == "ADDED_CHARACTER")
                    {
                        List<string> installedFiles = EnumerateFiles("./XVRebornTemp");
                        List<string> final = new List<string>();

                        foreach (string installedFile in installedFiles)
                        {
                            string modifiedFile = installedFile.Replace("./XVRebornTemp", Properties.Settings.Default.datafolder);
                            final.Add(modifiedFile);
                        }

                        string installedDir = Path.Combine(Settings.Default.datafolder, "installed");
                        if (!Directory.Exists(installedDir))
                        {
                            Directory.CreateDirectory(installedDir);
                        }

                        string filePath = Path.Combine(Settings.Default.datafolder, Settings.Default.datafolder + $"/installed/{modtype}_{modname}_{CMS_BCS}_{CharID}.txt");
                        File.WriteAllLines(filePath, final);

                       
                        if(Directory.Exists("./XVRebornTemp/JUNGLE"))
                            MergeDirectoriesWithConfirmation("./XVRebornTemp/JUNGLE", Settings.Default.datafolder);
                        MergeDirectoriesWithConfirmation("./XVRebornTemp", Settings.Default.datafolder);

                        Clean();


                        CMS cms = new CMS();
                        cms.Load(Settings.Default.datafolder + @"/system/char_model_spec.cms");
                        CharacterData newCharacter = new CharacterData
                        {
                            ID = CharID, // ID del personaggio
                            ShortName = CMS_BCS, // Nome abbreviato del personaggio
                            Unknown = new byte[8], // Array di byte sconosciuto
                            Paths = new string[7] // Array di percorsi
                        };
                        newCharacter.Paths[0] = CMS_BCS;
                        newCharacter.Paths[1] = CMS_EAN;
                        newCharacter.Paths[2] = CMS_FCE_EAN;
                        newCharacter.Paths[3] = CMS_CAM_EAN;
                        newCharacter.Paths[4] = CMS_BAC;
                        newCharacter.Paths[5] = CMS_BCM;
                        newCharacter.Paths[6] = CMS_BAI;
                        cms.AddCharacter(newCharacter);

                        // CSO
                        CSO cso = new CSO();
                        cso.Load(Settings.Default.datafolder + @"/system/chara_sound.cso");
                        CSO_Data characterData = new CSO_Data
                        {
                            Char_ID = CharID,           // Sostituisci con l'ID del personaggio desiderato
                            Costume_ID = 0,      // Sostituisci con l'ID del costume desiderato
                            Paths = new string[4]  // Aggiungi i percorsi desiderati
                            {
                                    CSO_1,
                                    CSO_2,
                                    CSO_3,
                                    CSO_4
                            }
                        };
                        cso.AddCharacter(characterData);

                        // CUS

                        Process p = new Process();
                        ProcessStartInfo info = new ProcessStartInfo();
                        info.FileName = "cmd.exe";
                        info.CreateNoWindow = true;
                        info.WindowStyle = ProcessWindowStyle.Hidden;
                        info.RedirectStandardInput = true;
                        info.UseShellExecute = false;
                        p.StartInfo = info;
                        p.Start();
                        using (StreamWriter sw = p.StandardInput)
                        {
                            if (sw.BaseStream.CanWrite)
                            {
                                sw.WriteLine("cd " + Settings.Default.datafolder + @"\system");
                                sw.WriteLine(@"XMLSerializer.exe custom_skill.cus");
                            }
                        }
                        p.WaitForExit();

                        string cuspath = Settings.Default.datafolder + @"\system\custom_skill.cus.xml";
                        string text4 = File.ReadAllText(cuspath);

                        text4 = text4.Replace("  </Skillsets>", "    <Skillset Character_ID=\"" + CharID + $"\" Costume_Index=\"0\" Model_Preset=\"0\">\r\n      <SuperSkill1 ID1=\"{CUS_SUPER_1}\" />\r\n      <SuperSkill2 ID1=\"{CUS_SUPER_2}\" />\r\n      <SuperSkill3 ID1=\"{CUS_SUPER_3}\" />\r\n      <SuperSkill4 ID1=\"{CUS_SUPER_4}\" />\r\n      <UltimateSkill1 ID1=\"{CUS_ULTIMATE_1}\" />\r\n      <UltimateSkill2 ID1=\"{CUS_ULTIMATE_2}\" />\r\n      <EvasiveSkill ID1=\"{CUS_EVASIVE}\" />\r\n      <BlastType ID1=\"65535\" />\r\n      <AwokenSkill ID1=\"0\" />\r\n    </Skillset>\r\n  </Skillsets>");
                        File.WriteAllText(cuspath, text4);

                        p.Start();

                        using (StreamWriter sw = p.StandardInput)
                        {
                            if (sw.BaseStream.CanWrite)
                            {
                                const string quote = "\"";

                                sw.WriteLine("cd " + Settings.Default.datafolder + @"\system");
                                sw.WriteLine(@"XMLSerializer.exe " + quote + Settings.Default.datafolder + @"\system\custom_skill.cus.xml" + quote);
                            }
                        }

                        p.WaitForExit();

                        ///
                        // AUR
                        info.FileName = "cmd.exe";
                        info.CreateNoWindow = true;
                        info.WindowStyle = ProcessWindowStyle.Hidden;
                        info.RedirectStandardInput = true;
                        info.UseShellExecute = false;
                        p.StartInfo = info;

                        p.Start();
                        using (StreamWriter sw = p.StandardInput)
                        {
                            if (sw.BaseStream.CanWrite)
                            {
                                sw.WriteLine("cd " + Settings.Default.datafolder + @"\system");
                                sw.WriteLine(@"XMLSerializer.exe aura_setting.aur");
                            }
                        }
                        p.WaitForExit();

                        string aurpath = Settings.Default.datafolder + @"\system\aura_setting.aur.xml";
                        string text5 = File.ReadAllText(aurpath);
                        string glare;
                        if (AUR_GLARE == 1)
                        {
                            glare = "True";
                        }
                        else
                        {
                            glare = "False";
                        }
                        text5 = text5.Replace("  </CharacterAuras>", "    <CharacterAura Chara_ID=\"" + CharID + $"\" Costume=\"0\" Aura_ID=\"{AUR_ID}\" Glare=\"{glare}\" />\r\n  </CharacterAuras>");
                        File.WriteAllText(aurpath, text5);

                        p.Start();

                        using (StreamWriter sw = p.StandardInput)
                        {
                            if (sw.BaseStream.CanWrite)
                            {
                                const string quote = "\"";

                                sw.WriteLine("cd " + Settings.Default.datafolder + @"\system");
                                sw.WriteLine(@"XMLSerializer.exe " + quote + Settings.Default.datafolder + @"\system\aura_setting.aur.xml" + quote);
                            }
                        }

                        p.WaitForExit();


                        //////

                        // PSC
                        p.Start();
                        using (StreamWriter sw = p.StandardInput)
                        {
                            if (sw.BaseStream.CanWrite)
                            {
                                sw.WriteLine("cd " + Settings.Default.datafolder + @"\system");
                                sw.WriteLine(@"XMLSerializer.exe parameter_spec_char.psc");
                            }
                        }
                        p.WaitForExit();

                        string pscpath = Settings.Default.datafolder + @"\system\parameter_spec_char.psc.xml";
                        string text6 = File.ReadAllText(pscpath);

                        text6 = text6.Replace("  </Configuration>\r\n</PSC>",
                            "    <PSC_Entry Chara_ID=\"" + CharID + "\">\r\n" +
                            "      <PscSpecEntry Costume=\"" + PSC_COSTUME + "\" Preset=\"" + PSC_PRESET + "\">\r\n" +
                            "        <Camera_Position value=\"" + PSC_CAMERA_POS + "\" />\r\n" +
                            "        <I_12 value=\"" + PSC_I_12 + "\" />\r\n" +
                            "        <Health value=\"" + PSC_HEALTH + "\" />\r\n" +
                            "        <F_20 value=\"" + PSC_F_20 + "\" />\r\n" +
                            "        <Ki value=\"" + PSC_KI + "\" />\r\n" +
                            "        <Ki_Recharge value=\"" + PSC_KI_RECHARGE + "\" />\r\n" +
                            "        <I_32 value=\"" + PSC_I_32 + "\" />\r\n" +
                            "        <I_36 value=\"" + PSC_I_36 + "\" />\r\n" +
                            "        <I_40 value=\"" + PSC_I_40 + "\" />\r\n" +
                            "        <Stamina value=\"" + PSC_STAMINA + "\" />\r\n" +
                            "        <Stamina_Recharge value=\"" + PSC_STAMINA_RECHARGE + "\" />\r\n" +
                            "        <F_52 value=\"" + PSC_F_52 + "\" />\r\n" +
                            "        <F_56 value=\"" + PSC_F_56 + "\" />\r\n" +
                            "        <I_60 value=\"" + PSC_I_60 + "\" />\r\n" +
                            "        <Basic_Atk_Defense value=\"" + PSC_BASIC_ATK_DEF + "\" />\r\n" +
                            "        <Basic_Ki_Defense value=\"" + PSC_BASIC_KI_DEF + "\" />\r\n" +
                            "        <Strike_Atk_Defense value=\"" + PSC_STRIKE_ATK_DEF + "\" />\r\n" +
                            "        <Super_Ki_Defense value=\"" + PSC_SUPER_KI_DEF + "\" />\r\n" +
                            "        <Ground_Speed value=\"" + PSC_GROUND_SPEED + "\" />\r\n" +
                            "        <Air_Speed value=\"" + PSC_AIR_SPEED + "\" />\r\n" +
                            "        <Boost_Speed value=\"" + PSC_BOOST_SPEED + "\" />\r\n" +
                            "        <Dash_Speed value=\"" + PSC_DASH_SPEED + "\" />\r\n" +
                            "        <F_96 value=\"" + PSC_F_96 + "\" />\r\n" +
                            "        <Reinforcement_Skill_Duration value=\"" + PSC_REINFORCEMENT_SKILL + "\" />\r\n" +
                            "        <F_104 value=\"" + PSC_F_104 + "\" />\r\n" +
                            "        <Revival_HP_Amount value=\"" + PSC_REVIVAL_HP_AMOUNT + "\" />\r\n" +
                            "        <Reviving_Speed value=\"" + PSC_REVIVAL_SPEED + "\" />\r\n" +
                            "        <F_116 value=\"" + PSC_F_116 + "\" />\r\n" +
                            "        <F_120 value=\"" + PSC_F_120 + "\" />\r\n" +
                            "        <F_124 value=\"" + PSC_F_124 + "\" />\r\n" +
                            "        <F_128 value=\"" + PSC_F_128 + "\" />\r\n" +
                            "        <F_132 value=\"" + PSC_F_132 + "\" />\r\n" +
                            "        <F_136 value=\"" + PSC_F_136 + "\" />\r\n" +
                            "        <I_140 value=\"" + PSC_I_140 + "\" />\r\n" +
                            "        <F_144 value=\"" + PSC_F_144 + "\" />\r\n" +
                            "        <F_148 value=\"" + PSC_F_148 + "\" />\r\n" +
                            "        <F_152 value=\"" + PSC_F_152 + "\" />\r\n" +
                            "        <F_156 value=\"" + PSC_F_156 + "\" />\r\n" +
                            "        <F_160 value=\"" + PSC_F_160 + "\" />\r\n" +
                            "        <F_164 value=\"" + PSC_F_164 + "\" />\r\n" +
                            "        <Z-Soul value=\"" + PSC_Z_SOUL + "\" />\r\n" +
                            "        <I_172 value=\"" + PSC_I_172 + "\" />\r\n" +
                            "        <I_176 value=\"" + PSC_I_176 + "\" />\r\n" +
                            "        <F_180 value=\"" + PSC_F_180 + "\" />\r\n" +
                            "      </PscSpecEntry>\r\n" +
                            "    </PSC_Entry>\r\n" +
                            "  </Configuration>\r\n</PSC>");
                        File.WriteAllText(pscpath, text6);

                        p.Start();

                        using (StreamWriter sw = p.StandardInput)
                        {
                            if (sw.BaseStream.CanWrite)
                            {
                                const string quote = "\"";

                                sw.WriteLine("cd " + Settings.Default.datafolder + @"\system");
                                sw.WriteLine(@"XMLSerializer.exe " + quote + Settings.Default.datafolder + @"\system\parameter_spec_char.psc.xml" + quote);
                            }
                        }

                        p.WaitForExit();
                        //////


                        string Charalist = Settings.Default.datafolder + @"\XVP_SLOTS.xs";

                        var text10 = new StringBuilder();

                        foreach (string s in File.ReadAllLines(Charalist))
                        {
                            text10.Append(s.Replace("{[JCO,0,0,0,110,111]}", "{[JCO,0,0,0,110,111]}{[" + CMS_BCS + $",0,0,0,{VOX_1},{VOX_2}]}}"));
                        }

                        using (var file1 = new StreamWriter(File.Create(Charalist)))
                        {
                            file1.Write(text10.ToString());
                        }

                        msg MSGfile;
                        MSGfile = msgStream.Load(Settings.Default.datafolder + @"/msg/proper_noun_character_name_" + language + ".msg");
                        msgData[] expand = new msgData[MSGfile.data.Length + 1];
                        Array.Copy(MSGfile.data, expand, MSGfile.data.Length);
                        string nameid = MSGfile.data[MSGfile.data.Length - 1].NameID;
                        int endid = int.Parse(nameid.Substring(nameid.Length - 3, 3));
                        expand[expand.Length - 1].ID = MSGfile.data.Length;
                        expand[expand.Length - 1].Lines = new string[] { MSG_CHARACTER_NAME };
                        expand[expand.Length - 1].NameID = "chara_" + CMS_BCS + "_" + (endid).ToString("000");
                        MSGfile.data = expand;
                        msgStream.Save(MSGfile, Settings.Default.datafolder + @"/msg/proper_noun_character_name_" + language + ".msg");

                        p.Start();

                        using (StreamWriter sw = p.StandardInput)
                        {
                            if (sw.BaseStream.CanWrite)
                            {
                                sw.WriteLine("cd " + Settings.Default.datafolder + @"\ui\texture");
                                sw.WriteLine(@"embpack.exe CHARA01");
                            }
                        }

                        string[] row = { modname, modauthor, "Added Character" };
                        ListViewItem lvi = new ListViewItem(row);
                        lvMods.Items.Add(lvi);
                        saveLvItems();
                    }
                    else if(modtype == "ADDED_SKILL")
                    {

                        List<string> installedFiles = EnumerateFiles("./XVRebornTemp");
                        List<string> final = new List<string>();

                        foreach (string installedFile in installedFiles)
                        {
                            string modifiedFile = installedFile.Replace("./XVRebornTemp", Properties.Settings.Default.datafolder);
                            final.Add(modifiedFile);
                        }

                        string installedDir = Path.Combine(Settings.Default.datafolder, "installed");
                        if (!Directory.Exists(installedDir))
                        {
                            Directory.CreateDirectory(installedDir);
                        }
                        //062_GHS_MSK

                        string filePath = Path.Combine(Settings.Default.datafolder, Settings.Default.datafolder + $"/installed/{modtype}_{modname}_{SKILL_TYPE}_{SKILL_ID1.PadLeft(3, '0')}_{SKILL_ID2.PadLeft(3, '0')}_{CharID}_{SKILL_ShortName}.txt");
                        File.WriteAllLines(filePath, final);


                        if (Directory.Exists("./XVRebornTemp/JUNGLE"))
                            MergeDirectoriesWithConfirmation("./XVRebornTemp/JUNGLE", Settings.Default.datafolder);
                        MergeDirectoriesWithConfirmation("./XVRebornTemp", Settings.Default.datafolder);

                        Clean();

                        //CUS

                        // CUS
                        Process p = new Process();
                        ProcessStartInfo info = new ProcessStartInfo();
                        info.FileName = "cmd.exe";
                        info.CreateNoWindow = true;
                        info.WindowStyle = ProcessWindowStyle.Hidden;
                        info.RedirectStandardInput = true;
                        info.UseShellExecute = false;
                        p.StartInfo = info;
                        p.Start();

                        using (StreamWriter sw = p.StandardInput)
                        {
                            if (sw.BaseStream.CanWrite)
                            {
                                sw.WriteLine("cd " + Settings.Default.datafolder + @"\system");
                                sw.WriteLine(@"XMLSerializer.exe custom_skill.cus");
                            }
                        }

                        p.WaitForExit();

                        string cuspath = Settings.Default.datafolder + @"\system\custom_skill.cus.xml";
                        string text4 = File.ReadAllText(cuspath);

                        switch (SKILL_TYPE)
                        {
                            case "SPA":
                                text4 = text4.Replace("\r\n  </SuperSkills>", $"<Skill ShortName=\"{SKILL_ShortName}\" ID1=\"{SKILL_ID1}\" ID2=\"{SKILL_ID2}\"><I_04 value=\"{SKILL_I_04}\" /><Race_Lock value=\"{SKILL_Race_Lock}\" /><Type value=\"{SKILL_Type}\" /><FilesLoaded Flags=\"{SKILL_FilesLoaded}\" /><PartSet value=\"{SKILL_PartSet}\" /><I_18 value=\"{SKILL_I_18}\" /><EAN Path=\"{SKILL_EAN}\" /><CAM_EAN Path=\"{SKILL_CAM_EAN}\" /><EEPK Path=\"{SKILL_EEPK}\" /><ACB_SE Path=\"{SKILL_ACB_SE}\" /><ACB_VOX Path=\"{SKILL_ACB_VOX}\" /><AFTER_BAC Path=\"{SKILL_AFTER_BAC}\" /><AFTER_BCM Path=\"{SKILL_AFTER_BCM}\" /><I_48 value=\"{SKILL_I_48}\" /><I_50 value=\"{SKILL_I_50}\" /><I_52 value=\"{SKILL_I_52}\" /><I_54 value=\"{SKILL_I_54}\" /><PUP ID=\"{SKILL_PUP}\" /><CUS_Aura value=\"{SKILL_CUS_Aura}\" /><TransformCharaSwap Chara_ID=\"{SKILL_TransformCharaSwap}\" /><Skillset_Change ModelPreset=\"{SKILL_Skillset_Change}\" /><Num_Of_Transforms value=\"{SKILL_Num_Of_Transforms}\" /><I_66 value=\"{SKILL_I_66}\" /></Skill></SuperSkills>");
                                break;
                            case "ULT":
                                text4 = text4.Replace("\r\n  </UltimateSkills>", $"<Skill ShortName=\"{SKILL_ShortName}\" ID1=\"{SKILL_ID1}\" ID2=\"{SKILL_ID2}\"><I_04 value=\"{SKILL_I_04}\" /><Race_Lock value=\"{SKILL_Race_Lock}\" /><Type value=\"{SKILL_Type}\" /><FilesLoaded Flags=\"{SKILL_FilesLoaded}\" /><PartSet value=\"{SKILL_PartSet}\" /><I_18 value=\"{SKILL_I_18}\" /><EAN Path=\"{SKILL_EAN}\" /><CAM_EAN Path=\"{SKILL_CAM_EAN}\" /><EEPK Path=\"{SKILL_EEPK}\" /><ACB_SE Path=\"{SKILL_ACB_SE}\" /><ACB_VOX Path=\"{SKILL_ACB_VOX}\" /><AFTER_BAC Path=\"{SKILL_AFTER_BAC}\" /><AFTER_BCM Path=\"{SKILL_AFTER_BCM}\" /><I_48 value=\"{SKILL_I_48}\" /><I_50 value=\"{SKILL_I_50}\" /><I_52 value=\"{SKILL_I_52}\" /><I_54 value=\"{SKILL_I_54}\" /><PUP ID=\"{SKILL_PUP}\" /><CUS_Aura value=\"{SKILL_CUS_Aura}\" /><TransformCharaSwap Chara_ID=\"{SKILL_TransformCharaSwap}\" /><Skillset_Change ModelPreset=\"{SKILL_Skillset_Change}\" /><Num_Of_Transforms value=\"{SKILL_Num_Of_Transforms}\" /><I_66 value=\"{SKILL_I_66}\" /></Skill></UltimateSkills>");
                                break;
                            case "ESC":
                                text4 = text4.Replace("\r\n  </EvasiveSkills>", $"<Skill ShortName=\"{SKILL_ShortName}\" ID1=\"{SKILL_ID1}\" ID2=\"{SKILL_ID2}\"><I_04 value=\"{SKILL_I_04}\" /><Race_Lock value=\"{SKILL_Race_Lock}\" /><Type value=\"{SKILL_Type}\" /><FilesLoaded Flags=\"{SKILL_FilesLoaded}\" /><PartSet value=\"{SKILL_PartSet}\" /><I_18 value=\"{SKILL_I_18}\" /><EAN Path=\"{SKILL_EAN}\" /><CAM_EAN Path=\"{SKILL_CAM_EAN}\" /><EEPK Path=\"{SKILL_EEPK}\" /><ACB_SE Path=\"{SKILL_ACB_SE}\" /><ACB_VOX Path=\"{SKILL_ACB_VOX}\" /><AFTER_BAC Path=\"{SKILL_AFTER_BAC}\" /><AFTER_BCM Path=\"{SKILL_AFTER_BCM}\" /><I_48 value=\"{SKILL_I_48}\" /><I_50 value=\"{SKILL_I_50}\" /><I_52 value=\"{SKILL_I_52}\" /><I_54 value=\"{SKILL_I_54}\" /><PUP ID=\"{SKILL_PUP}\" /><CUS_Aura value=\"{SKILL_CUS_Aura}\" /><TransformCharaSwap Chara_ID=\"{SKILL_TransformCharaSwap}\" /><Skillset_Change ModelPreset=\"{SKILL_Skillset_Change}\" /><Num_Of_Transforms value=\"{SKILL_Num_Of_Transforms}\" /><I_66 value=\"{SKILL_I_66}\" /></Skill></EvasiveSkills>");
                                break; 
                        }

                        File.WriteAllText(cuspath, text4);

                        p.Start();

                        using (StreamWriter sw = p.StandardInput)
                        {
                            if (sw.BaseStream.CanWrite)
                            {
                                const string quote = "\"";

                                sw.WriteLine("cd " + Settings.Default.datafolder + @"\system");
                                sw.WriteLine(@"XMLSerializer.exe " + quote + Settings.Default.datafolder + @"\system\custom_skill.cus.xml" + quote);
                            }
                        }

                        p.WaitForExit();

                        //MSG

                        msg MSGfile;
                        msg MSGfile2;
                        MSGfile = msgStream.Load(Settings.Default.datafolder + @"/msg/proper_noun_skill_" + "spa" + "_name_" + language + ".msg");
                        MSGfile2 = msgStream.Load(Settings.Default.datafolder + @"/msg/proper_noun_skill_" + "spa" + "_info_" + language + ".msg");
                        string MSGfileSkName = "";
                        string MSGfileSkInfo = "";
                        string sktypemsg = "";

                        switch (SKILL_TYPE)
                        {
                            case "SPA":
                                sktypemsg = "spe_skill_";
                                MSGfileSkName = Settings.Default.datafolder + @"/msg/proper_noun_skill_" + "spa" + "_name_" + language + ".msg";
                                MSGfileSkInfo = Settings.Default.datafolder + @"/msg/proper_noun_skill_" + "spa" + "_info_" + language + ".msg";
                                MSGfile = msgStream.Load(Settings.Default.datafolder + @"/msg/proper_noun_skill_" + "spa" + "_name_" + language + ".msg");
                                MSGfile2 = msgStream.Load(Settings.Default.datafolder + @"/msg/proper_noun_skill_" + "spa" + "_info_" + language + ".msg");
                                break;
                            case "ULT":
                                sktypemsg = "ult_";

                                MSGfileSkName = Settings.Default.datafolder + @"/msg/proper_noun_skill_" + "ult" + "_name_" + language + ".msg";
                                MSGfileSkInfo = Settings.Default.datafolder + @"/msg/proper_noun_skill_" + "ult" + "_info_" + language + ".msg";
                                MSGfile = msgStream.Load(Settings.Default.datafolder + @"/msg/proper_noun_skill_" + "ult" + "_name_" + language + ".msg");
                                MSGfile2 = msgStream.Load(Settings.Default.datafolder + @"/msg/proper_noun_skill_" + "ult" + "_info_" + language + ".msg");
                                break;
                            case "ESC":
                                sktypemsg = "avoid_skill_";

                                MSGfileSkName = Settings.Default.datafolder + @"/msg/proper_noun_skill_" + "esc" + "_name_" + language + ".msg";
                                MSGfileSkInfo = Settings.Default.datafolder + @"/msg/proper_noun_skill_" + "esc" + "_info_" + language + ".msg";
                                MSGfile = msgStream.Load(Settings.Default.datafolder + @"/msg/proper_noun_skill_" + "esc" + "_name_" + language + ".msg");
                                MSGfile2 = msgStream.Load(Settings.Default.datafolder + @"/msg/proper_noun_skill_" + "esc" + "_info_" + language + ".msg");
                                break;

                        }
                        msgData[] expand = new msgData[MSGfile.data.Length + 1];;
                        Array.Copy(MSGfile.data, expand, MSGfile.data.Length);
                        string nameid = MSGfile.data[MSGfile.data.Length - 1].NameID;
                        int endid = int.Parse(nameid.Substring(nameid.Length - 3, 3));
                        expand[expand.Length - 1].ID = MSGfile.data.Length;
                        expand[expand.Length - 1].Lines = new string[] { MSG_SKILL_NAME };
                        expand[expand.Length - 1].NameID = sktypemsg + SKILL_ID1.PadLeft(3, '0');
                        MSGfile.data = expand;
                        msgStream.Save(MSGfile, MSGfileSkName);

                        expand = new msgData[MSGfile2.data.Length + 1];
                        Array.Copy(MSGfile2.data, expand, MSGfile2.data.Length);
                        nameid = MSGfile2.data[MSGfile2.data.Length - 1].NameID;
                        endid = int.Parse(nameid.Substring(nameid.Length - 3, 3));
                        expand[expand.Length - 1].ID = MSGfile2.data.Length;
                        expand[expand.Length - 1].Lines = new string[] { MSG_SKILL_DESC };
                        expand[expand.Length - 1].NameID = sktypemsg + SKILL_ID1.PadLeft(3, '0');
                        MSGfile2.data = expand;
                        msgStream.Save(MSGfile2, MSGfileSkName);

                        Clean();
                        string[] row = { modname, modauthor, "Added Skill" };
                        ListViewItem lvi = new ListViewItem(row);
                        lvMods.Items.Add(lvi);
                        saveLvItems();
                    }
                }
            }
            Clean();
        }
        private void uninstallMod(object sender, EventArgs e)
        {
            if (lvMods.SelectedItems.Count == 0)
            {
                MessageBox.Show("No mod selected for uninstallation.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }
            string serializerPath = Settings.Default.datafolder + "/system/XMLSerializer.exe";

            ListViewItem selectedItem = lvMods.SelectedItems[0];
            string modtype = selectedItem.SubItems[2].Text;
            string modname = selectedItem.SubItems[0].Text;
            switch (modtype)
            {
                case "Replacer":
                    modtype = "REPLACER";
                    break;
                case "Added Character":
                    modtype = "ADDED_CHARACTER";
                    break;
                case "Added Skill":
                    modtype = "ADDED_SKILL";
                    break;
                default:
                    throw new NotImplementedException();
            }
            string modFilePath = Path.Combine(Settings.Default.datafolder, "installed", $"{modtype}_{modname}.txt");

            if (modtype == "REPLACER")
            {
                if (File.Exists(modFilePath))
                {
                    string[] installedFiles = File.ReadAllLines(modFilePath);
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
                    File.Delete(modFilePath);
                }
            }
            else if (modtype == "ADDED_CHARACTER")
            {
                string modFilePathPattern = Path.Combine(Settings.Default.datafolder, "installed", $"{modtype}_{modname}_*_*.txt");

                // Use Directory.GetFiles with the pattern to find matching files
                string[] matchingFiles = Directory.GetFiles(Settings.Default.datafolder + "/installed", $"{modtype}_{modname}_*_*.txt");

                // If you need to work with the first matching file, for example
                modFilePath = matchingFiles.Length > 0 ? matchingFiles[0] : null; // Handle the case if no file is found

                if (modFilePath != null)
                {
                    Console.WriteLine("Found file: " + modFilePath);
                }
                else
                {
                    Console.WriteLine("No matching file found.");
                }
                string fileName = Path.GetFileName(modFilePath);
                // Updated pattern to allow spaces, parentheses, and other special characters
                string pattern = @"^(\w+)_(.*?)_(\w+)_(\d+)\.txt$";

                // Match the file name against the pattern
                Match match = Regex.Match(fileName, pattern);
                string CMS_BCS = "";
                int CMS_ID = 0;
                if (match.Success)
                {
                    CMS_BCS = match.Groups[3].Value;
                    if(int.TryParse(match.Groups[4].Value, out CMS_ID))
                        Console.WriteLine($"Found CMS_BCS: {CMS_BCS}, CMS_ID: {CMS_ID}");
                }
                else
                {
                    Console.WriteLine("No matching file format found.");
                }

                if (File.Exists(modFilePath))
                {
                    string[] installedFiles = File.ReadAllLines(modFilePath);
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
                    File.Delete(modFilePath);
                }

                // Remove Character from CMS
                CMS cms = new CMS();
                cms.Load(Settings.Default.datafolder + @"/system/char_model_spec.cms");
                for (int i = 0; i < cms.Data.Count(); i++)
                {
                    if (cms.Data[i].ShortName == CMS_BCS)
                        cms.RemoveCharacter(cms.Data[i]);
                }


                // Remove Character from CSO
                CSO cso = new CSO();
                cso.Load(Settings.Default.datafolder + @"/system/chara_sound.cso");
                for (int i = 0; i < cso.Data.Count(); i++)
                {
                    if (cso.Data[i].Char_ID == CMS_ID)
                        cso.RemoveCharacter(cso.Data[i]);
                }
                // Remove Character from CUS
                RunCommand($"\"{serializerPath}\" \"{Settings.Default.datafolder + @"/system/custom_skill.cus"}\"");

                string cuspath = Settings.Default.datafolder + @"/system/custom_skill.cus.xml";
                if (File.Exists(cuspath))
                {
                    // Read the content of the CUS XML file
                    string text4 = File.ReadAllText(cuspath);

                    // Define the regex pattern to remove the skillset
                    string skillsetToRemove = $"    <Skillset Character_ID=\"{CMS_ID}\" Costume_Index=\"0\" Model_Preset=\"0\">.*?</Skillset>";

                    // Perform the regex replacement
                    text4 = Regex.Replace(text4, skillsetToRemove, "", RegexOptions.Singleline);

                    // Write the updated content back to the file
                    File.WriteAllText(cuspath, text4);
                }

                // Re-serialize the CUS file
                RunCommand($"\"{serializerPath}\" \"{cuspath}\"");

                // Remove Character from AUR
                RunCommand($"\"{serializerPath}\" \"{Settings.Default.datafolder + @"/system/aura_setting.aur"}\"");

                string aurpath = Settings.Default.datafolder + @"/system/aura_setting.aur.xml";
                if (File.Exists(aurpath))
                {
                    // Read the content of the AUR XML file
                    string text5 = File.ReadAllText(aurpath);

                    // Define the regex pattern to remove the aura entry
                    string auraToRemove = $"    <CharacterAura Chara_ID=\"{CMS_ID}\" Costume=\"0\".*?/>\r\n";

                    // Perform the regex replacement
                    text5 = Regex.Replace(text5, auraToRemove, "");

                    // Write the updated content back to the file
                    File.WriteAllText(aurpath, text5);
                }

                // Re-serialize the AUR file
                RunCommand($"\"{serializerPath}\" \"{Settings.Default.datafolder + @"/system/aura_setting.aur.xml"}\"");

                // Remove Character from PSC
                RunCommand($"\"{serializerPath}\" \"{Settings.Default.datafolder + @"/system/parameter_spec_char.psc"}\"");

                string pscpath = Settings.Default.datafolder + @"/system/parameter_spec_char.psc.xml";
                if (File.Exists(pscpath))
                {
                    // Read the content of the PSC XML file
                    string text6 = File.ReadAllText(pscpath);

                    // Define the regex pattern to remove the PSC entry
                    string pscToRemove = $"    <PSC_Entry Chara_ID=\"{CMS_ID}\">.*?</PSC_Entry>";

                    // Perform the regex replacement
                    text6 = Regex.Replace(text6, pscToRemove, "", RegexOptions.Singleline);

                    // Write the updated content back to the file
                    File.WriteAllText(pscpath, text6);
                }
                RunCommand($"\"{serializerPath}\" \"{Settings.Default.datafolder + @"/system/parameter_spec_char.psc.xml"}\"");

                string charalist = Settings.Default.datafolder + @"/XVP_SLOTS.xs";
                if (File.Exists(charalist))
                {
                    string s = File.ReadAllText(charalist);
                    Console.WriteLine("CMS_BCS value: " + CMS_BCS);
                    string escapedCMS_BCS = Regex.Escape(CMS_BCS);
                    Console.WriteLine("Escaped CMS_BCS: " + escapedCMS_BCS);
                    pattern = @"\{\[\s*" + escapedCMS_BCS + @"\s*,\s*0\s*,\s*0\s*,\s*0\s*,\s*(-?\d{1,3})\s*,\s*(-?\d{1,3})\s*\]\}";
                    Console.WriteLine("Regex pattern: " + pattern);
                    string modifiedContent = Regex.Replace(s, pattern, "");

                    Console.WriteLine("Modified content: " + modifiedContent);
                    File.WriteAllText(charalist, modifiedContent);
                }
                else
                {
                    Console.WriteLine("File does not exist.");
                }
                // Remove from MSG
                string msgPath = Settings.Default.datafolder + @"/msg/proper_noun_character_name_" + language + ".msg";
                if (File.Exists(msgPath))
                {
                    msg MSGfile = msgStream.Load(msgPath);
                    List<msgData> updatedData = MSGfile.data.Where(d => !d.NameID.Contains(CMS_BCS)).ToList();
                    MSGfile.data = updatedData.ToArray();
                    msgStream.Save(MSGfile, msgPath);
                }


            }
            else if (modtype == "ADDED_SKILL")
            {
                string modFilePathPattern = Path.Combine(Settings.Default.datafolder, "installed", $"{modtype}_{modname}_*_*_*.txt");

                // Use Directory.GetFiles with the pattern to find matching files
                string[] matchingFiles = Directory.GetFiles(Settings.Default.datafolder + "/installed", $"{modtype}_{modname}_*_*_*_*.txt");

                // If you need to work with the first matching file, for example
                modFilePath = matchingFiles.Length > 0 ? matchingFiles[0] : null; // Handle the case if no file is found

                if (modFilePath != null)
                {
                    Console.WriteLine("Found file: " + modFilePath);
                }
                else
                {
                    Console.WriteLine("No matching file found.");
                }
                // Check if the file exists
                if (File.Exists(modFilePath))
                {
                    // Get the list of files installed
                    string[] installedFiles = File.ReadAllLines(modFilePath);

                    // Delete the installed files
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

                    // Delete the mod installation data file
                    File.Delete(modFilePath);
                }

                // Remove the directories created during installation
                if (Directory.Exists("./XVRebornTemp/JUNGLE"))
                    Directory.Delete("./XVRebornTemp/JUNGLE", true);
                if (Directory.Exists("./XVRebornTemp"))
                    Directory.Delete("./XVRebornTemp", true);

                string fileName = Path.GetFileName(modFilePath);
                
                string pattern = @"^(\w+)_(\w+)_(\w+)_(\d{3})_(\w+)_(\w+)_(.*?)\.txt$";

                // Match the file name against the pattern
                Match match = Regex.Match(fileName, pattern);


                string SKILL_TYPE = "";
                string SKILL_ID1 = "";
                string SKILL_ID2 = "";
                string CharID = "";
                string SKILL_ShortName = "";

                if (match.Success)
                {
                    // Extract the values from the matching groups
                    SKILL_TYPE = match.Groups[3].Value;         // The third group corresponds to SKILL_TYPE
                    SKILL_ID1 = match.Groups[4].Value;          // The fourth group corresponds to SKILL_ID1 (padded with zeros)
                    SKILL_ID2 = match.Groups[5].Value;          // The fourth group corresponds to SKILL_ID1 (padded with zeros)
                    CharID = match.Groups[6].Value;             // The fifth group corresponds to CharID
                    SKILL_ShortName = match.Groups[7].Value;    // The sixth group corresponds to SKILL_ShortName

                    Console.WriteLine($"SKILL_TYPE: {SKILL_TYPE}");
                    Console.WriteLine($"SKILL_ID1: {SKILL_ID1}");
                    Console.WriteLine($"SKILL_ID2: {SKILL_ID2}");
                    Console.WriteLine($"CharID: {CharID}");
                    Console.WriteLine($"SKILL_ShortName: {SKILL_ShortName}");

                    // Example of constructing the file path
                    string filePath = Path.Combine(Settings.Default.datafolder, $"{Settings.Default.datafolder}/installed/{modtype}_{modname}_{SKILL_TYPE}_{SKILL_ID1.PadLeft(3, '0')}_{SKILL_ID2.PadLeft(3, '0')}_{CharID}_{SKILL_ShortName}.txt");
                    Console.WriteLine($"File path: {filePath}");
                }
                else
                {
                    Console.WriteLine("No matching file format found.");
                }
                // Run the first command (assuming it's required for some processing)
                RunCommand($"\"{serializerPath}\" \"{Settings.Default.datafolder + @"/system/custom_skill.cus"}\"");

                // Remove the custom skill from the XML file
                string cuspath = Settings.Default.datafolder + @"\system\custom_skill.cus.xml";
                if (File.Exists(cuspath))
                {
                    // Read the content of the XML file
                    string xmlContent = File.ReadAllText(cuspath);

                    // Define the pattern to match the skill to remove (accounting for the exact padding)
                    string patternCUS = $@"<Skill ShortName=""{SKILL_ShortName}"" ID1=""{SKILL_ID1.PadLeft(3, '0')}"" ID2=""{SKILL_ID2.PadLeft(3, '0')}"">.*?</Skill>";

                    // Remove the skill from the XML content (using the single-line option to handle multiline)
                    string updatedXmlContent = Regex.Replace(xmlContent, patternCUS, string.Empty, RegexOptions.Singleline);

                    // Write the updated XML back to the file
                    File.WriteAllText(cuspath, updatedXmlContent);

                    // Finally, serialize the updated XML (if this step is needed)
                    RunCommand($"\"{serializerPath}\" \"{Settings.Default.datafolder + @"/system/custom_skill.cus.xml"}\"");
                }


                // Remove skill data from the MSG files
                string msgPathName = Settings.Default.datafolder + @"/msg/proper_noun_skill_" + SKILL_TYPE.ToLower() + "_name_" + language + ".msg";
                string msgPathInfo = Settings.Default.datafolder + @"/msg/proper_noun_skill_" + SKILL_TYPE.ToLower() + "_info_" + language + ".msg";

                if (File.Exists(msgPathName))
                {
                    msg MSGfile = msgStream.Load(msgPathName);
                    List<msgData> updatedNameData = MSGfile.data.Where(d => !d.NameID.Contains(SKILL_ID1.PadLeft(3, '0'))).ToList();
                    MSGfile.data = updatedNameData.ToArray();
                    msgStream.Save(MSGfile, msgPathName);
                }

                if (File.Exists(msgPathInfo))
                {
                    msg MSGfile2 = msgStream.Load(msgPathInfo);
                    List<msgData> updatedInfoData = MSGfile2.data.Where(d => !d.NameID.Contains(SKILL_ID1.PadLeft(3, '0'))).ToList();
                    MSGfile2.data = updatedInfoData.ToArray();
                    msgStream.Save(MSGfile2, msgPathInfo);
                }

                // Remove the mod from the ListView and save the updated list
                lvMods.Items.Remove(selectedItem);
                saveLvItems();
            }


            lvMods.Items.Remove(selectedItem);
            saveLvItems();
            MessageBox.Show("Mod successfully uninstalled.", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
            Clean();
        }

        public static void RemoveEmptyDirectories(string path)
        {
            if (Directory.Exists(path))
            {
                foreach (var subdirectory in Directory.GetDirectories(path))
                {
                    RemoveEmptyDirectories(subdirectory); 
                }

                if (Directory.GetFileSystemEntries(path).Length == 0)
                {
                    try
                    {
                        Directory.Delete(path); // Remove the empty directory
                        Console.WriteLine($"Removed empty directory: {path}");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error removing directory {path}: {ex.Message}");
                    }
                }
            }
            else
            {
                Console.WriteLine($"Directory not found: {path}");
            }
        }
        public static void MergeDirectoriesWithConfirmation(string sourceDir, string destDir)
        {
            if (!Directory.Exists(sourceDir) || !Directory.Exists(destDir))
            {
                throw new DirectoryNotFoundException("Source or destination directory does not exist.");
            }

            string[] sourceSubDirs = Directory.GetDirectories(sourceDir);

            foreach (string sourceFile in Directory.GetFiles(sourceDir))
            {
                string fileName = Path.GetFileName(sourceFile);
                string destFile = Path.Combine(destDir, fileName);

                if (File.Exists(destFile))
                {
                    var result = MessageBox.Show($"A file with the name '{fileName}' already exists. Do you want to replace it?", "File Replace Confirmation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        File.Copy(sourceFile, destFile, true); // Replace the existing file.
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        return; // Cancel the entire operation.
                    }
                }
                else
                {
                    File.Copy(sourceFile, destFile);
                }
            }

            // Recursively merge the subdirectories.
            foreach (string sourceSubDir in sourceSubDirs)
            {
                string dirName = Path.GetFileName(sourceSubDir);
                string destSubDir = Path.Combine(destDir, dirName);

                // If the destination subdirectory doesn't exist, create it.
                if (!Directory.Exists(destSubDir))
                {
                    Directory.CreateDirectory(destSubDir);
                }

                // Recursively merge this subdirectory.
                MergeDirectoriesWithConfirmation(sourceSubDir, destSubDir);
            }
        }


        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void editCHARASELEToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form3 frm = new Form3();
            frm.ShowDialog();
        }
        public void RunCommand(string command)
        {
            Process p = new Process();
            ProcessStartInfo info = new ProcessStartInfo
            {
                FileName = "cmd.exe",
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                RedirectStandardInput = true,
                UseShellExecute = false
            };

            p.StartInfo = info;
            p.Start();

            using (StreamWriter sw = p.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    sw.WriteLine(command);
                }
            }

            p.WaitForExit();
        }
        private void ConvertX2M(object sender, EventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Convert X2M Mod";
            ofd.Filter = "Xenoverse 2 Mod Files (*.x2m)|*.x2m";
            ofd.Multiselect = false;  //Important
            ofd.CheckFileExists = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in ofd.FileNames)
                {
                    string tempFolder = "./XVRebornTemp";
                    Directory.CreateDirectory(tempFolder);
                    ZipFile.ExtractToDirectory(ofd.FileName, tempFolder);

                    var x2mfolder = Directory.GetDirectories(tempFolder).FirstOrDefault(d => Regex.IsMatch(Path.GetFileName(d), "^[A-Z0-9]{3}$"));
                    if (x2mfolder != null)
                    {
                        X2M x2m = new X2M();

                        foreach (var sfile in Directory.GetFiles(x2mfolder, "*", SearchOption.AllDirectories))
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
                    string embpackPath = Path.Combine(Settings.Default.datafolder, @"ui\texture", "embpack.exe");

                    string finalPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\{Path.GetFileNameWithoutExtension(ofd.FileName)}";
                    MoveDirectory(tempFolder, finalPath, true);
                    // Helper method to safely get attribute value
                    string GetAttributeValue(XmlNode node, string attributeName)
                    {
                        var attribute = node?.Attributes[attributeName];
                        return attribute?.Value ?? "N/A"; // Return "N/A" if the attribute doesn't exist
                    }
                    var xmlContent = File.ReadAllText(finalPath + @"/x2m.xml");
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlContent);
                    // Basic Information
                    string x2mType = GetAttributeValue(doc.SelectSingleNode("//X2M"), "type");

                    if(x2mType == "NEW_CHARACTER")
                    {
                        string ddsFolder = "";
                        Match match = Regex.Match(ddsFolder, @"character_(\d+)_info");
                        // Get directories whose name length is 3
                        var matchingDirectories = Directory.GetDirectories(tempFolder)
                            .Where(dir => Path.GetFileName(dir).Length == 3)
                            .ToList();

                        // If you want to take the first matching directory (if any)
                        if (matchingDirectories.Any())
                        {
                            ddsFolder = matchingDirectories.First();
                            Console.WriteLine(ddsFolder);
                        }
                        else
                        {
                            Console.WriteLine("No matching directories found.");
                        }
                        string[] embFiles = Directory.GetFiles(ddsFolder, "*.dyt.emb", SearchOption.AllDirectories);
                        Console.WriteLine($"Found {embFiles.Length} EMB files in {ddsFolder}");
                        if (!Directory.Exists(ddsFolder))
                        {
                            Console.WriteLine($"Folder not found: {ddsFolder}");
                            return;
                        }
                        foreach (string embfile in embFiles)
                        {
                            RunCommand($"\"{embpackPath}\" \"{embfile}\"");
                            string[] ddsFiles = Directory.GetFiles(ddsFolder, "*.dds", SearchOption.AllDirectories);

                            if (ddsFiles.Length == 0)
                            {
                                Console.WriteLine("No DDS files found in the specified folder.");
                                return;
                            }

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

                            foreach (var group in groupedByFolder)
                            {
                                string folder = group.Key;
                                List<string> ddsFilesInFolder = group.Value;

                                Console.WriteLine($"Processing folder: {folder}");

                                foreach (string ddsFile in ddsFilesInFolder)
                                {
                                    Console.WriteLine($"Cleaning DDS file: {ddsFile}");
                                    DDS.CleanDDSForXV1(ddsFile, ddsFile);
                                }

                                if (!Directory.Exists(folder))
                                {
                                    Console.WriteLine($"Directory does not exist for embFilePath: {folder}");
                                    continue;
                                }

                                RunCommand($"\"{embpackPath}\" \"{folder}\"");
                                Directory.Delete(folder, true);
                            }
                        }
                        string formatVersion = GetAttributeValue(doc.SelectSingleNode("//X2M_FORMAT_VERSION"), "value");
                        string modName = GetAttributeValue(doc.SelectSingleNode("//MOD_NAME"), "value");
                        string modAuthor = GetAttributeValue(doc.SelectSingleNode("//MOD_AUTHOR"), "value");
                        string modVersion = GetAttributeValue(doc.SelectSingleNode("//MOD_VERSION"), "value");
                        string modGuid = GetAttributeValue(doc.SelectSingleNode("//MOD_GUID"), "value");
                        string uData = GetAttributeValue(doc.SelectSingleNode("//UDATA"), "value");
                        string entryName = GetAttributeValue(doc.SelectSingleNode("//ENTRY_NAME"), "value");
                        string charaNameEn = GetAttributeValue(doc.SelectSingleNode("//CHARA_NAME_EN"), "value");

                        // SlotEntry
                        string slotCostumeIndex = GetAttributeValue(doc.SelectSingleNode("//SlotEntry"), "costume_index");
                        string modelPreset = GetAttributeValue(doc.SelectSingleNode("//SlotEntry/MODEL_PRESET"), "value");
                        string flagGK2 = GetAttributeValue(doc.SelectSingleNode("//SlotEntry/FLAG_GK2"), "value");
                        string voicesIdList = GetAttributeValue(doc.SelectSingleNode("//SlotEntry/VOICES_ID_LIST"), "value");
                        string costumeNameEn = GetAttributeValue(doc.SelectSingleNode("//SlotEntry/COSTUME_NAME_EN"), "value");

                        // Entry
                        string entryId = GetAttributeValue(doc.SelectSingleNode("//Entry"), "id");
                        string entryNameId = GetAttributeValue(doc.SelectSingleNode("//Entry"), "name");

                        string u10 = GetAttributeValue(doc.SelectSingleNode("//Entry/U_10"), "value");
                        string loadCamDist = GetAttributeValue(doc.SelectSingleNode("//Entry/LOAD_CAM_DIST"), "value");
                        string u16 = GetAttributeValue(doc.SelectSingleNode("//Entry/U_16"), "value");
                        string u18 = GetAttributeValue(doc.SelectSingleNode("//Entry/U_18"), "value");
                        string u1A = GetAttributeValue(doc.SelectSingleNode("//Entry/U_1A"), "value");
                        string character = GetAttributeValue(doc.SelectSingleNode("//Entry/CHARACTER"), "value");
                        string ean = GetAttributeValue(doc.SelectSingleNode("//Entry/EAN"), "value");
                        string fceEan = GetAttributeValue(doc.SelectSingleNode("//Entry/FCE_EAN"), "value");
                        string fce = GetAttributeValue(doc.SelectSingleNode("//Entry/FCE"), "value");
                        string camEan = GetAttributeValue(doc.SelectSingleNode("//Entry/CAM_EAN"), "value");
                        string bac = GetAttributeValue(doc.SelectSingleNode("//Entry/BAC"), "value");
                        string bcm = GetAttributeValue(doc.SelectSingleNode("//Entry/BCM"), "value");
                        string ai = GetAttributeValue(doc.SelectSingleNode("//Entry/AI"), "value");
                        string str50 = GetAttributeValue(doc.SelectSingleNode("//Entry/STR_50"), "value");

                        // SkillSet
                        string skillSetCharId = GetAttributeValue(doc.SelectSingleNode("//SkillSet/CHAR_ID"), "value");
                        string skillSetCostumeId = GetAttributeValue(doc.SelectSingleNode("//SkillSet/COSTUME_ID"), "value");
                        string skillsValue = GetAttributeValue(doc.SelectSingleNode("//SkillSet/SKILLS"), "value");

                        // Split the SKILLS string by commas
                        string[] skills = skillsValue.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                        // Output the parsed skills
                        Console.WriteLine("Skills: ");
                        foreach (var skille in skills)
                        {
                            Console.WriteLine(skille.Trim()); // Trim any extra whitespace
                        }
                        string skillSetModelPreset = GetAttributeValue(doc.SelectSingleNode("//SkillSet/MODEL_PRESET"), "value");

                        // CsoEntry
                        string csoCharId = GetAttributeValue(doc.SelectSingleNode("//CsoEntry/CHAR_ID"), "value");
                        string csoCostumeId = GetAttributeValue(doc.SelectSingleNode("//CsoEntry/COSTUME_ID"), "value");
                        string se = GetAttributeValue(doc.SelectSingleNode("//CsoEntry/SE"), "value");
                        string vox = GetAttributeValue(doc.SelectSingleNode("//CsoEntry/VOX"), "value");
                        string amk = GetAttributeValue(doc.SelectSingleNode("//CsoEntry/AMK"), "value");
                        string csoSkills = GetAttributeValue(doc.SelectSingleNode("//CsoEntry/SKILLS"), "value");

                        // PscSpecEntry
                        string costumeId = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/COSTUME_ID"), "value");
                        string costumeId2 = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/COSTUME_ID2"), "value");
                        string cameraPosition = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/CAMERA_POSITION"), "value");
                        string u0C = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/U_0C"), "value");
                        string u10_2 = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/U_10"), "value");
                        string health = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/HEALTH"), "value");
                        string f18 = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/F_18"), "value");
                        string ki = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/KI"), "value");
                        string kiRecharge = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/KI_RECHARGE"), "value");
                        string u24 = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/U_24"), "value");
                        string u28 = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/U_28"), "value");
                        string u2C = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/U_2C"), "value");
                        string stamina = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/STAMINA"), "value");
                        string staminaRechargeMove = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/STAMINA_RECHARGE_MOVE"), "value");
                        string staminaRechargeAir = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/STAMINA_RECHARGE_AIR"), "value");
                        string staminaRechargeGround = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/STAMINA_RECHARGE_GROUND"), "value");
                        string staminaDrainRate1 = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/STAMINA_DRAIN_RATE1"), "value");
                        string staminaDrainRate2 = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/STAMINA_DRAIN_RATE2"), "value");
                        string f48 = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/F_48"), "value");
                        string basicAttack = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/BASIC_ATTACK"), "value");
                        string basicKiAttack = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/BASIC_KI_ATTACK"), "value");
                        string strikeAttack = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/STRIKE_ATTACK"), "value");
                        string kiBlastSuper = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/KI_BLAST_SUPER"), "value");
                        string basicPhysDefense = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/BASIC_PHYS_DEFENSE"), "value");
                        string basicKiDefense = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/BASIC_KI_DEFENSE"), "value");
                        string strikeAtkDefense = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/STRIKE_ATK_DEFENSE"), "value");
                        string superKiBlastDefense = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/SUPER_KI_BLAST_DEFENSE"), "value");
                        string groundSpeed = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/GROUND_SPEED"), "value");
                        string airSpeed = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/AIR_SPEED"), "value");
                        string boostingSpeed = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/BOOSTING_SPEED"), "value");
                        string dashDistance = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/DASH_DISTANCE"), "value");
                        string f7C = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/F_7C"), "value");
                        string reinfSkillDuration = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/REINF_SKILL_DURATION"), "value");
                        string f84 = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/F_84"), "value");
                        string revivalHpAmount = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/REVIVAL_HP_AMOUNT"), "value");
                        string f8C = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/F_8C"), "value");
                        string revivingSpeed = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/REVIVING_SPEED"), "value");
                        string u98 = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/U_98"), "value");
                        string talisman = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/TALISMAN"), "value");
                        string uB8 = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/U_B8"), "value");
                        string uBC = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/U_BC"), "value");
                        string fC0 = GetAttributeValue(doc.SelectSingleNode("//PscSpecEntry/F_C0"), "value");

                        // CharacLink
                        string characLinkIdCharac = GetAttributeValue(doc.SelectSingleNode("//CharacLink"), "idCharac");
                        string characLinkIdCostume = GetAttributeValue(doc.SelectSingleNode("//CharacLink"), "idCostume");
                        string characLinkIdAura = GetAttributeValue(doc.SelectSingleNode("//CharacLink"), "idAura");
                        string characLinkGlare = GetAttributeValue(doc.SelectSingleNode("//CharacLink"), "glare");

                        // SevEntryHL
                        string sevEntryHlCostumeId = GetAttributeValue(doc.SelectSingleNode("//SevEntryHL"), "costume_id");
                        string sevEntryHlCopyChar = GetAttributeValue(doc.SelectSingleNode("//SevEntryHL"), "copy_char");
                        string sevEntryHlCopyCostume = GetAttributeValue(doc.SelectSingleNode("//SevEntryHL"), "copy_costume");

                        // CmlEntry
                        string cmlEntryCharId = GetAttributeValue(doc.SelectSingleNode("//CmlEntry"), "char_id");
                        string cmlEntryCostumeId = GetAttributeValue(doc.SelectSingleNode("//CmlEntry"), "costume_id");
                        string cmlEntryU04 = GetAttributeValue(doc.SelectSingleNode("//CmlEntry/U_04"), "value");
                        string cmlEntryCssPos = GetAttributeValue(doc.SelectSingleNode("//CmlEntry/CSS_POS"), "value");
                        string cmlEntryCssRot = GetAttributeValue(doc.SelectSingleNode("//CmlEntry/CSS_ROT"), "value");
                        string cmlEntryF0C = GetAttributeValue(doc.SelectSingleNode("//CmlEntry/F_0C"), "value");
                        string cmlEntryF10 = GetAttributeValue(doc.SelectSingleNode("//CmlEntry/F_10"), "value");
                        string cmlEntryF14 = GetAttributeValue(doc.SelectSingleNode("//CmlEntry/F_14"), "value");
                        string cmlEntryF18 = GetAttributeValue(doc.SelectSingleNode("//CmlEntry/F_18"), "value");
                        string cmlEntryF1C = GetAttributeValue(doc.SelectSingleNode("//CmlEntry/F_1C"), "value");
                        string cmlEntryF20 = GetAttributeValue(doc.SelectSingleNode("//CmlEntry/F_20"), "value");
                        string cmlEntryF24 = GetAttributeValue(doc.SelectSingleNode("//CmlEntry/F_24"), "value");
                        string cmlEntryF28 = GetAttributeValue(doc.SelectSingleNode("//CmlEntry/F_28"), "value");
                        string cmlEntryF2C = GetAttributeValue(doc.SelectSingleNode("//CmlEntry/F_2C"), "value");

                        File.Delete(finalPath + @"/x2m.xml");
                        // Create an XmlWriterSettings instance for formatting the XML
                        XmlWriterSettings settings = new XmlWriterSettings
                        {
                            Indent = true,
                            IndentChars = "    ", // Use four spaces for indentation
                        };

                        // Create the XmlWriter and write the XML content
                        string xmlFilePath = finalPath + @"/xvmod.xml";
                        using (XmlWriter writer = XmlWriter.Create(xmlFilePath, settings))
                        {
                            writer.WriteStartDocument();
                            writer.WriteStartElement("XVMOD");
                            writer.WriteAttributeString("type", "ADDED_CHARACTER");

                            WriteElementWithValue(writer, "MOD_NAME", modName);
                            WriteElementWithValue(writer, "MOD_AUTHOR", modAuthor);


                            WriteElementWithValue(writer, "AUR_ID", Convert.ToInt32(characLinkIdAura, 16).ToString());
                            if (characLinkGlare == "true")
                                WriteElementWithValue(writer, "AUR_GLARE", "1");
                            else
                                WriteElementWithValue(writer, "AUR_GLARE", "0");

                            WriteElementWithValue(writer, "CMS_BCS", character);
                            WriteElementWithValue(writer, "CMS_EAN", ean);
                            WriteElementWithValue(writer, "CMS_FCE_EAN", fceEan);
                            WriteElementWithValue(writer, "CMS_CAM_EAN", camEan);
                            WriteElementWithValue(writer, "CMS_BAC", bac);
                            WriteElementWithValue(writer, "CMS_BCM", bcm);
                            WriteElementWithValue(writer, "CMS_BAI", ai);

                            WriteElementWithValue(writer, "CSO_1", se);
                            WriteElementWithValue(writer, "CSO_2", vox);
                            WriteElementWithValue(writer, "CSO_3", amk);
                            WriteElementWithValue(writer, "CSO_4", csoSkills);

                            WriteElementWithValue(writer, "CUS_SUPER_1", skills[0]);
                            WriteElementWithValue(writer, "CUS_SUPER_2", skills[1]);
                            WriteElementWithValue(writer, "CUS_SUPER_3", skills[2]);
                            WriteElementWithValue(writer, "CUS_SUPER_4", skills[3]);
                            WriteElementWithValue(writer, "CUS_ULTIMATE_1", skills[4]);
                            WriteElementWithValue(writer, "CUS_ULTIMATE_2", skills[5]);
                            WriteElementWithValue(writer, "CUS_EVASIVE", skills[6]);


                            WriteElementWithValue(writer, "PSC_COSTUME", "0");
                            WriteElementWithValue(writer, "PSC_PRESET", "0");
                            WriteElementWithValue(writer, "PSC_CAMERA_POSITION", cameraPosition);
                            WriteElementWithValue(writer, "PSC_HEALTH", health);
                            WriteElementWithValue(writer, "PSC_I_12", "0");
                            WriteElementWithValue(writer, "PSC_F_20", "0");

                            WriteElementWithValue(writer, "PSC_KI", ki);
                            WriteElementWithValue(writer, "PSC_KI_RECHARGE", kiRecharge);
                            WriteElementWithValue(writer, "PSC_I_32", "0");
                            WriteElementWithValue(writer, "PSC_I_36", "0");
                            WriteElementWithValue(writer, "PSC_I_40", "0");
                            WriteElementWithValue(writer, "PSC_STAMINA", stamina);

                            WriteElementWithValue(writer, "PSC_STAMINA_RECHARGE", staminaRechargeGround);
                            WriteElementWithValue(writer, "PSC_F_52", "0");
                            WriteElementWithValue(writer, "PSC_F_56", "0");
                            WriteElementWithValue(writer, "PSC_I_60", "0");
                            WriteElementWithValue(writer, "PSC_BASIC_ATK_DEF", basicPhysDefense);
                            WriteElementWithValue(writer, "PSC_BASIC_KI_DEF", basicKiDefense);

                            WriteElementWithValue(writer, "PSC_STRIKE_ATK_DEF", strikeAtkDefense);
                            WriteElementWithValue(writer, "PSC_SUPER_KI_DEF", superKiBlastDefense);
                            WriteElementWithValue(writer, "PSC_GROUND_SPEED", groundSpeed);
                            WriteElementWithValue(writer, "PSC_AIR_SPEED", airSpeed);
                            WriteElementWithValue(writer, "PSC_BOOST_SPEED", boostingSpeed);

                            WriteElementWithValue(writer, "PSC_DASH_SPEED", dashDistance);
                            WriteElementWithValue(writer, "PSC_F96", "0");
                            WriteElementWithValue(writer, "PSC_REINFORCEMENT_SKILL", reinfSkillDuration);
                            WriteElementWithValue(writer, "PSC_F104", "0");
                            WriteElementWithValue(writer, "PSC_REVIVAL_HP_AMOUNT", revivalHpAmount);

                            WriteElementWithValue(writer, "PSC_REVIVING_SPEED", revivingSpeed);
                            WriteElementWithValue(writer, "PSC_F_116", "0");
                            WriteElementWithValue(writer, "PSC_F_120", "0");
                            WriteElementWithValue(writer, "PSC_F_124", "0");
                            WriteElementWithValue(writer, "PSC_F_128", "0");

                            WriteElementWithValue(writer, "PSC_F_132", "0");
                            WriteElementWithValue(writer, "PSC_F_136", "0");
                            WriteElementWithValue(writer, "PSC_I_140", "0");
                            WriteElementWithValue(writer, "PSC_F_144", "0");
                            WriteElementWithValue(writer, "PSC_F_148", "0");

                            WriteElementWithValue(writer, "PSC_F_152", "0");
                            WriteElementWithValue(writer, "PSC_F_156", "0");
                            WriteElementWithValue(writer, "PSC_F_160", "0");
                            WriteElementWithValue(writer, "PSC_F_164", "0");
                            WriteElementWithValue(writer, "PSC_Z_SOUL", "0");

                            WriteElementWithValue(writer, "PSC_I_172", "0");
                            WriteElementWithValue(writer, "PSC_I_176", "0");
                            WriteElementWithValue(writer, "PSC_F_180", "0");


                            WriteElementWithValue(writer, "MSG_CHARACTER_NAME", charaNameEn);
                            WriteElementWithValue(writer, "MSG_COSTUME_NAME", costumeNameEn);

                            WriteElementWithValue(writer, "VOX_1", "-1");
                            WriteElementWithValue(writer, "VOX_2", "-1");

                            writer.WriteEndElement(); // Close XVMOD
                            writer.WriteEndDocument(); // Close the document
                        }
                        Directory.CreateDirectory(finalPath + $"/ui/texture/CHARA01/");
                        File.Move(finalPath + @"/UI/SEL.DDS", finalPath + $"/ui/texture/CHARA01/{character}_000.DDS");
                        MoveDirectory(finalPath + $"/{character}", finalPath + $"/chara/{character}", true);
                        ZipFile.CreateFromDirectory(finalPath, finalPath + ".xvmod");
                    }
                    else if(x2mType == "NEW_SKILL")
                    {


                    }
                    MessageBox.Show($"X2M Converted successfully, you can find the converted file in \"{finalPath + ".xvmod"}\"", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Clean();
                }

            }

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
        public static void MoveDirectory(string sourceDir, string destDir, bool recursive)  //Just to keep it the same
        {
            // Ensure the source directory exists
            if (!Directory.Exists(sourceDir))
            {
                throw new DirectoryNotFoundException($"Source directory not found: {sourceDir}");
            }

            // If the destination exists, delete it to prevent conflicts
            if (Directory.Exists(destDir))
            {
                Directory.Delete(destDir, recursive);
            }

            // Create the destination directory
            Directory.CreateDirectory(destDir);

            // Move all files
            foreach (string file in Directory.GetFiles(sourceDir, "*", SearchOption.AllDirectories))
            {
                string destFile = file.Replace(sourceDir, destDir);
                Directory.CreateDirectory(Path.GetDirectoryName(destFile)); // Ensure subdirectories exist
                File.Move(file, destFile);
            }

            // Remove the source directory after everything has been moved
            Directory.Delete(sourceDir, recursive);
        }


    }
}