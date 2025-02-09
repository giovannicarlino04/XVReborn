using FreeImageAPI;
using Newtonsoft.Json;
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
                            text10.AppendLine(s.Replace("{[JCO,0,0,0,110,111]}", "{[JCO,0,0,0,110,111]}{[" + CMS_BCS + $",0,0,0,{VOX_1},{VOX_2}]}}"));
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
                    else
                    {

                        MessageBox.Show("File already exists in data folder, cannot proceed with installation.", "Error",
                            MessageBoxButtons.OK, MessageBoxIcon.Error);
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


                // Remove Character from CMS
                CSO cso = new CSO();
                cso.Load(Settings.Default.datafolder + @"/system/chara_sound.cso");
                for (int i = 0; i < cso.Data.Count(); i++)
                {
                    if (cso.Data[i].Char_ID == CMS_ID)
                        cso.RemoveCharacter(cso.Data[i]);
                }


                // Remove Character from CUS
                string cuspath = Settings.Default.datafolder + @"/system/custom_skill.cus.xml";
                if (File.Exists(cuspath))
                {
                    string text4 = File.ReadAllText(cuspath);
                    string skillsetToRemove = $"    <Skillset Character_ID=\"{CMS_ID}\" Costume_Index=\"0\" Model_Preset=\"0\">.*?</Skillset>";
                    text4 = Regex.Replace(text4, skillsetToRemove, "", RegexOptions.Singleline);
                    File.WriteAllText(cuspath, text4);
                }

                // Remove Character from AUR
                string aurpath = Settings.Default.datafolder + @"/system/aura_setting.aur.xml";
                if (File.Exists(aurpath))
                {
                    string text5 = File.ReadAllText(aurpath);
                    string auraToRemove = $"    <CharacterAura Chara_ID=\"{CMS_ID}\" Costume=\"0\".*?/>\r\n";
                    text5 = Regex.Replace(text5, auraToRemove, "");
                    File.WriteAllText(aurpath, text5);
                }

                // Remove Character from PSC
                string pscpath = Settings.Default.datafolder + @"/system/parameter_spec_char.psc.xml";
                if (File.Exists(pscpath))
                {
                    string text6 = File.ReadAllText(pscpath);
                    string pscToRemove = $"    <PSC_Entry Chara_ID=\"{CMS_ID}\">.*?</PSC_Entry>";
                    text6 = Regex.Replace(text6, pscToRemove, "", RegexOptions.Singleline);
                    File.WriteAllText(pscpath, text6);
                }

                // Remove from XVP_SLOTS.xs
                string charalist = Settings.Default.datafolder + @"/XVP_SLOTS.xs";
                if (File.Exists(charalist))
                {
                    var text10 = new StringBuilder();
                    foreach (string s in File.ReadAllLines(charalist))
                    {
                        if (!s.Contains(CMS_BCS))
                        {
                            text10.AppendLine(s);
                        }
                    }
                    File.WriteAllText(charalist, text10.ToString());
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

                    string ddsFolder = tempFolder;
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
                    string finalPath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop) + $"\\{Path.GetFileNameWithoutExtension(ofd.FileName)}";
                    MoveDirectory(tempFolder, finalPath, true);
                    MessageBox.Show($"X2M Converted successfully, you can find the converted files in \"{finalPath}\"", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    Clean();
                }
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