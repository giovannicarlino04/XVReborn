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
using System.Threading;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;
using XVReborn.Properties;
using static System.Net.Mime.MediaTypeNames;

namespace XVReborn
{

    public partial class Form1 : Form
    {
        string language = "";

        public Form1()
        {
            InitializeComponent();
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
                var myStream8 = myAssembly.GetManifestResourceStream("XVReborn.ZipFile_Blobs.XV2CONV.zip");

                ZipArchive archive = new ZipArchive(myStream);
                ZipArchive archive2 = new ZipArchive(myStream2);
                ZipArchive archive3 = new ZipArchive(myStream3);
                ZipArchive archive4 = new ZipArchive(myStream4);
                ZipArchive archive5 = new ZipArchive(myStream5);
                ZipArchive archive6 = new ZipArchive(myStream6);
                ZipArchive archive7 = new ZipArchive(myStream7);
                ZipArchive archive8 = new ZipArchive(myStream8);

                archive.ExtractToDirectory(Path.Combine(Settings.Default.datafolder + @"\system"));
                archive2.ExtractToDirectory(Path.Combine(Settings.Default.datafolder + @"\system"));
                archive3.ExtractToDirectory(Path.Combine(Settings.Default.datafolder + @"\system"));
                archive4.ExtractToDirectory(Path.Combine(Settings.Default.datafolder + @"\system"));
                archive5.ExtractToDirectory(Path.Combine(Settings.Default.datafolder + @"\system"));
                archive6.ExtractToDirectory(Path.Combine(Settings.Default.datafolder + @"\system"));
                archive7.ExtractToDirectory(Path.Combine(Settings.Default.datafolder + @"\system"));
                archive8.ExtractToDirectory(Path.Combine(Settings.Default.datafolder + @"\system"));
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
            string MSG_CHARACTER_NAME = "";
            string MSG_COSTUME_NAME = "";
            short VOX_1 = -1;
            short VOX_2 = -1;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in ofd.FileNames)
                {
                    string xmlfile = "./XVRebornTemp" + @"/xvmod.xml";

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

                        MergeDirectoriesWithConfirmation("./XVRebornTemp", Settings.Default.datafolder);

                        Clean();


                        string[] row = { modname, modauthor, "Replacer" };
                        ListViewItem lvi = new ListViewItem(row);
                        lvMods.Items.Add(lvi);
                        saveLvItems();
                    }
                    else if (modtype == "ADDED_CHARACTER")
                    {
                        int CharID = 108 + Settings.Default.modlist.Count;
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
                        CharSkill skill = new CharSkill();
                        skill.populateSkillData(Settings.Default.datafolder + @"/msg", Settings.Default.datafolder + @"/system/custom_skill.cus", language);

                        if (!short.TryParse(CUS_SUPER_1, out short super1)) super1 = -1;
                        if (!short.TryParse(CUS_SUPER_2, out short super2)) super2 = -1;
                        if (!short.TryParse(CUS_SUPER_3, out short super3)) super3 = -1;
                        if (!short.TryParse(CUS_SUPER_4, out short super4)) super4 = -1;
                        if (!short.TryParse(CUS_ULTIMATE_1, out short ultimate1)) ultimate1 = -1;
                        if (!short.TryParse(CUS_ULTIMATE_2, out short ultimate2)) ultimate2 = -1;
                        if (!short.TryParse(CUS_EVASIVE, out short evasive)) evasive = -1;

                        Char_Data newChar = new Char_Data
                        {
                            charID = CharID, // ID del personaggio
                            CostumeID = 0, // ID del costume
                            SuperIDs = new short[] { super1, super2, super3, super4 }, // Super attacchi
                            UltimateIDs = new short[] { ultimate1, ultimate2 }, // Ultimate attacchi
                            EvasiveID = evasive // Attacco evasivo
                        };

                        // Controlla se il file è stato caricato correttamente prima di aggiungere il personaggio
                        if (skill.Chars != null)
                        {
                            skill.AddCharacter(newChar);
                        }


                        // AUR
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

                        text6 = text6.Replace("  </Configuration>\r\n</PSC>", "    <PSC_Entry Chara_ID=\"" + CharID + "\">\r\n      <PscSpecEntry Costume=\"0\" Preset=\"0\">\r\n        <Camera_Position value=\"1\" />\r\n        <I_12 value=\"5\" />\r\n        <Health value=\"1.1155\" />\r\n        <F_20 value=\"1.0\" />\r\n        <Ki value=\"1.0\" />\r\n        <Ki_Recharge value=\"1.0\" />\r\n        <I_32 value=\"1\" />\r\n        <I_36 value=\"1\" />\r\n        <I_40 value=\"0\" />\r\n        <Stamina value=\"1.5\" />\r\n        <Stamina_Recharge value=\"0.75\" />\r\n        <F_52 value=\"1.0\" />\r\n        <F_56 value=\"1.1\" />\r\n        <I_60 value=\"0\" />\r\n        <Basic_Atk_Defense value=\"1.0\" />\r\n        <Basic_Ki_Defense value=\"0.95\" />\r\n        <Strike_Atk_Defense value=\"1.1\" />\r\n        <Super_Ki_Defense value=\"0.95\" />\r\n        <Ground_Speed value=\"1.0\" />\r\n        <Air_Speed value=\"1.0\" />\r\n        <Boost_Speed value=\"1.0\" />\r\n        <Dash_Speed value=\"1.0\" />\r\n        <F_96 value=\"1.0\" />\r\n        <Reinforcement_Skill_Duration value=\"1.0\" />\r\n        <F_104 value=\"1.0\" />\r\n        <Revival_HP_Amount value=\"1.0\" />\r\n        <Reviving_Speed value=\"1.0\" />\r\n        <F_116 value=\"1.0\" />\r\n        <F_120 value=\"0.55\" />\r\n        <F_124 value=\"1.0\" />\r\n        <F_128 value=\"1.0\" />\r\n        <F_132 value=\"1.0\" />\r\n        <F_136 value=\"1.0\" />\r\n        <I_140 value=\"0\" />\r\n        <F_144 value=\"1.0\" />\r\n        <F_148 value=\"1.0\" />\r\n        <F_152 value=\"1.0\" />\r\n        <F_156 value=\"1.0\" />\r\n        <F_160 value=\"1.0\" />\r\n        <F_164 value=\"1.0\" />\r\n        <Z-Soul value=\"98\" />\r\n        <I_172 value=\"1\" />\r\n        <I_176 value=\"1\" />\r\n        <F_180 value=\"8.0\" />\r\n      </PscSpecEntry>\r\n    </PSC_Entry>\r\n  </Configuration>\r\n</PSC>");
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

        public static void MergeDirectoriesWithConfirmation(string sourceDir, string destDir)
        {
            if (!Directory.Exists(sourceDir) || !Directory.Exists(destDir))
            {
                throw new DirectoryNotFoundException("Source or destination directory does not exist.");
            }

            // Get the subdirectories in the source directory.
            string[] sourceSubDirs = Directory.GetDirectories(sourceDir);

            // Copy the files from the source to the destination.
            foreach (string sourceFile in Directory.GetFiles(sourceDir))
            {
                string fileName = Path.GetFileName(sourceFile);
                string destFile = Path.Combine(destDir, fileName);

                if (File.Exists(destFile))
                {
                    // Ask for confirmation to replace the existing file.
                    var result = MessageBox.Show($"A file with the name '{fileName}' already exists. Do you want to replace it?", "File Replace Confirmation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning);

                    if (result == DialogResult.Yes)
                    {
                        File.Copy(sourceFile, destFile, true); // Replace the existing file.
                    }
                    else if (result == DialogResult.Cancel)
                    {
                        return; // Cancel the entire operation.
                    }
                    // If 'No' is chosen, the existing file will not be replaced.
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

        static void ProcessBCS(string filePath)
        {


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
                    sw.WriteLine("\"XMLSerializer.exe\" " + "\"" + Path.GetFullPath(filePath) + "\"");

                }
            }
            p.WaitForExit();

            string xmlPath = filePath + ".xml";
            if (!File.Exists(xmlPath))
            {
                Console.WriteLine($"File not found: {xmlPath}");
                return;
            }

            // Carica il file XML
            XDocument doc = XDocument.Load(xmlPath);

            // Trova il nodo <BCS>
            XElement bcsElement = doc.Root;
            if (bcsElement == null || bcsElement.Name != "BCS")
            {
                Console.WriteLine("Invalid XML format: Missing <BCS> root element.");
                return;
            }

            // Leggi l'attributo "Version"
            XAttribute versionAttr = bcsElement.Attribute("Version");

            if (versionAttr != null)
            {
                Console.WriteLine($"Current Version: {versionAttr.Value}");

                // Modifica la versione da "XV2" a "XV1" se necessario
                if (versionAttr.Value.Equals("XV2", StringComparison.OrdinalIgnoreCase))
                {
                    versionAttr.Value = "XV1";
                    doc.Save(xmlPath);
                    Console.WriteLine($"Version changed to: {versionAttr.Value}");
                }
                else
                {
                    Console.WriteLine("No changes needed.");
                }
            }
            else
            {
                Console.WriteLine("Version attribute not found.");
            }
            p.Start();
            using (StreamWriter sw = p.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    sw.WriteLine("cd " + Settings.Default.datafolder + @"\system");
                    sw.WriteLine("\"XMLSerializer.exe\" " + "\"" + Path.GetFullPath(filePath) + ".xml\"");

                }
            }
            p.WaitForExit();
            File.Delete(Path.GetFullPath(filePath) + ".xml");
            Console.WriteLine($"Processed {filePath} (BCS)");
             
        }

        static void ChangeModelVer(string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
            using (var reader = new BinaryReader(stream))
            using (var writer = new BinaryWriter(stream))
            {
                stream.Seek(8, SeekOrigin.Begin);
                UInt32 version = reader.ReadUInt32();
                if (version == 37507 || version == 37508 || version == 37568) // 0x9274
                {
                    stream.Seek(8, SeekOrigin.Begin);
                    writer.Write(65537);
                    Console.WriteLine($"Processed {filePath}");
                }
            }
        }
        static void ChangeShaderHeader(string filePath)
        {
            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
            using (var reader = new BinaryReader(stream))
            using (var writer = new BinaryWriter(stream))
            {

                stream.Seek(6, SeekOrigin.Begin);
                byte[] newHeader = { 0x10, 0x00, 0x00, 0x00, 0x00, 0x00, 0x10, 0x00, 0x00, 0x00 };
                writer.Write(newHeader);

                Console.WriteLine($"Processed {filePath}");
            }
        }
        static void ChangeShaderVer(string filePath)
        {


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
                    sw.WriteLine("\"XMLSerializer.exe\" " + "\"" + Path.GetFullPath(filePath) + "\"");

                }
            }
            p.WaitForExit();

            string xmlPath = filePath + ".xml";
            if (!File.Exists(xmlPath))
            {
                Console.WriteLine($"File not found: {xmlPath}");
                return;
            }    // Cerca tutti i file .xml nella cartella

            try
            {
                // Carica il file XML
                XDocument doc = XDocument.Load(xmlPath);

                // Trova il nodo <BCS>
                XElement bcsElement = doc.Root;
                if (bcsElement == null || bcsElement.Name != "EMM")
                {
                    Console.WriteLine("Invalid XML format: Missing <EMM> root element.");
                    return;
                }
                // Leggi l'attributo "Version"
                XAttribute versionAttr = bcsElement.Attribute("Version");

                if (versionAttr != null)
                {
                    Console.WriteLine($"Current Version: {versionAttr.Value}");

                    if (versionAttr.Value.Equals("37508", StringComparison.OrdinalIgnoreCase) || versionAttr.Value.Equals("37568", StringComparison.OrdinalIgnoreCase))
                    {
                        versionAttr.Value = "0";
                        Console.WriteLine($"Version changed in: {xmlPath}");
                    }
                    else
                    {
                        Console.WriteLine("No changes needed.");
                    }
                }
                else
                {
                    Console.WriteLine("Version attribute not found.");
                }


                // Salva il file modificato
                doc.Save(xmlPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing {xmlPath}: {ex.Message}");
            }
         
            p.Start();
            using (StreamWriter sw = p.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    sw.WriteLine("cd " + Settings.Default.datafolder + @"\system");
                    sw.WriteLine("\"XMLSerializer.exe\" " + "\"" + Path.GetFullPath(filePath) + ".xml\"");

                }
            }
            p.WaitForExit();
            File.Delete(Path.GetFullPath(filePath) + ".xml");
            Console.WriteLine($"Processed {filePath} (EMM)");

        }
        static void ChangeImageVer(string filePath)
        {


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
                    sw.WriteLine("\"XMLSerializer.exe\" " + "\"" + Path.GetFullPath(filePath) + "\"");

                }
            }
            p.WaitForExit();

            string xmlPath = filePath + ".xml";
            if (!File.Exists(xmlPath))
            {
                Console.WriteLine($"File not found: {xmlPath}");
                return;
            }    // Cerca tutti i file .xml nella cartella

            try
            {
                // Carica il file XML
                XDocument doc = XDocument.Load(xmlPath);

                XElement bcsElement = doc.Root;
                if (bcsElement == null || bcsElement.Name != "EMB_File")
                {
                    Console.WriteLine("Invalid XML format: Missing <EMB_File> root element.");
                    return;
                }
                // Leggi l'attributo "Version"
                XAttribute versionAttr = bcsElement.Attribute("I_08");

                if (versionAttr != null)
                {
                    Console.WriteLine($"Current Version: {versionAttr.Value}");

                    if (versionAttr.Value.Equals("37508", StringComparison.OrdinalIgnoreCase) || versionAttr.Value.Equals("37568", StringComparison.OrdinalIgnoreCase))
                    {
                        versionAttr.Value = "0";
                        Console.WriteLine($"Version changed in: {xmlPath}");
                    }
                    else
                    {
                        Console.WriteLine("No changes needed.");
                    }
                }
                else
                {
                    Console.WriteLine("Version attribute not found.");
                }
                // Salva il file modificato
                doc.Save(xmlPath);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error processing {xmlPath}: {ex.Message}");
            }

            p.Start();
            using (StreamWriter sw = p.StandardInput)
            {
                if (sw.BaseStream.CanWrite)
                {
                    sw.WriteLine("cd " + Settings.Default.datafolder + @"\system");
                    sw.WriteLine("\"XMLSerializer.exe\" " + "\"" + Path.GetFullPath(filePath) + ".xml\"");

                }
            }
            p.WaitForExit();
            File.Delete(Path.GetFullPath(filePath) + ".xml");
            Console.WriteLine($"Processed {filePath} (EMM)");

        }
        static void ProcessBAC(string filePath)
        {


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
                    sw.WriteLine("\"XV2Conv.exe\" " + "\"" + Path.GetFullPath(filePath) + "\"");

                }
            }
            p.WaitForExit();

            Console.WriteLine($"Processed {filePath} (EMM)");

        }
        static void RunCommand(string command)
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
        static void ExtractX2M(string zipFilePath, string outputFolder)
        {
            ZipFile.ExtractToDirectory(zipFilePath, outputFolder);
        }

        static void ProcessX2M(string x2mFilePath)
        {
            string tempFolder = "./XVRebornTemp";
            Directory.CreateDirectory(tempFolder);
            ExtractX2M(x2mFilePath, tempFolder);

            var folder = Directory.GetDirectories(tempFolder).FirstOrDefault(d => Regex.IsMatch(Path.GetFileName(d), "^[A-Z0-9]{3}$"));
            if (folder != null)
            {
                ProcessFiles(folder);
            }
        }

        static void ProcessFiles(string folder)
        {
            if (!Directory.Exists(folder))
            {
                Console.WriteLine($"Invalid folder: {folder}");
                return;
            }

            foreach (var file in Directory.GetFiles(folder, "*", SearchOption.AllDirectories))
            {
                switch (Path.GetExtension(file))
                {
                    case ".bcs":
                        ProcessBCS(file);
                        break;
                    case ".bac":
                    case ".bdm":
                        ProcessBAC(file);
                        break;
                    case ".emd":
                    case ".esk":
                    case ".ean":
                        ChangeModelVer(file);
                        break;
                    case ".emm":
                        ChangeShaderVer(file);
                        ChangeShaderHeader(file);
                        break;
                    case ".dyt.emb":
                        ChangeImageVer(file);
                        break;
                }


                if (File.Exists(file + @".xml"))
                    File.Delete(file + @".xml");
            }
        }
        private void installx2m(object sender, EventArgs e)
        {

            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Title = "Install Mod";
            ofd.Filter = "Xenoverse 2 Mod Files (*.x2m)|*.x2m";
            ofd.Multiselect = true;  //Important
            ofd.CheckFileExists = true;

            if (ofd.ShowDialog() == DialogResult.OK)
            {
                foreach (string file in ofd.FileNames)
                {
                    string xmlfile = "./XVRebornTemp" + @"/x2m.xml";
                    ProcessX2M(ofd.FileName);


                    string xmlContent = File.ReadAllText(xmlfile);

                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml(xmlContent);

                    XmlNode x2mNode = doc.SelectSingleNode("//X2M");
                    string x2mType = x2mNode.Attributes["type"].Value;
                    Console.WriteLine($"X2M Type: {x2mType}");
                    // Helper method to safely get attribute value
                    string GetAttributeValue(XmlNode node, string attributeName)
                    {
                        var attribute = node?.Attributes[attributeName];
                        return attribute?.Value ?? "N/A"; // Return "N/A" if the attribute doesn't exist
                    }

                    // Basic Information
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


                    if (x2mType != "NEW_CHARACTER")
                        return;  //Handle with a Msg maybe?

                    int CharID = 108 + Settings.Default.modlist.Count;
                    MergeDirectoriesWithConfirmation("./XVRebornTemp", Settings.Default.datafolder);

                    Clean();


                    CMS cms = new CMS();
                    cms.Load(Settings.Default.datafolder + @"/system/char_model_spec.cms");
                    CharacterData newCharacter = new CharacterData
                    {
                        ID = CharID, // ID del personaggio
                        ShortName = entryName, // Nome abbreviato del personaggio
                        Unknown = new byte[8], // Array di byte sconosciuto
                        Paths = new string[7] // Array di percorsi
                    };
                    newCharacter.Paths[0] = entryName;
                    newCharacter.Paths[1] = ean;
                    newCharacter.Paths[2] = fceEan;
                    newCharacter.Paths[3] = camEan;
                    newCharacter.Paths[4] = bac;
                    newCharacter.Paths[5] = bcm;
                    newCharacter.Paths[6] = ai;
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
                                    se,
                                    vox,
                                    amk,
                                    csoSkills,
                        }
                    };
                    cso.AddCharacter(characterData);

                    // CUS
                    CharSkill skill = new CharSkill();
                    skill.populateSkillData(Settings.Default.datafolder + @"/msg", Settings.Default.datafolder + @"/system/custom_skill.cus", language);

                    if (!short.TryParse(skills[0], out short super1)) super1 = -1;
                    if (!short.TryParse(skills[1], out short super2)) super2 = -1;
                    if (!short.TryParse(skills[2], out short super3)) super3 = -1;
                    if (!short.TryParse(skills[3], out short super4)) super4 = -1;
                    if (!short.TryParse(skills[4], out short ultimate1)) ultimate1 = -1;
                    if (!short.TryParse(skills[5], out short ultimate2)) ultimate2 = -1;
                    if (!short.TryParse(skills[6], out short evasive)) evasive = -1;

                    Char_Data newChar = new Char_Data
                    {
                        charID = CharID, // ID del personaggio
                        CostumeID = 0, // ID del costume
                        SuperIDs = new short[] { super1, super2, super3, super4 }, // Super attacchi
                        UltimateIDs = new short[] { ultimate1, ultimate2 }, // Ultimate attacchi
                        EvasiveID = evasive // Attacco evasivo
                    };

                    // Controlla se il file è stato caricato correttamente prima di aggiungere il personaggio
                    if (skill.Chars != null)
                    {
                        skill.AddCharacter(newChar);
                    }


                    // AUR
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
                            sw.WriteLine(@"XMLSerializer.exe aura_setting.aur");
                        }
                    }
                    p.WaitForExit();

                    string aurpath = Settings.Default.datafolder + @"\system\aura_setting.aur.xml";
                    string text5 = File.ReadAllText(aurpath);
                    string glare;

                    text5 = text5.Replace("  </CharacterAuras>", "    <CharacterAura Chara_ID=\"" + CharID + $"\" Costume=\"0\" Aura_ID=\"{characLinkIdAura}\" Glare=\"{characLinkGlare}\" />\r\n  </CharacterAuras>");
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

                    // Assuming the parsed values are stored in variables like these:

                    string staminaRecharge = "1.0";
                    string basicAtkDefense = "0.0";
                    string superKiDefense = "0.0";
                    string dashSpeed = "0.400000006";
                    string zSoul = "98";  // Example
                                          // Add other values as needed

                    // Replace the placeholders with parsed values in the XML format
                    text6 = text6.Replace("  </Configuration>\r\n</PSC>", $@"
                <PSC_Entry Chara_ID=""{CharID}"">
                  <PscSpecEntry Costume=""0"" Preset=""0"">
                    <Camera_Position value=""1"" />
                    <I_12 value=""5"" />
                    <Health value=""{health}"" />
                    <F_20 value=""1.0"" />
                    <Ki value=""{ki}"" />
                    <Ki_Recharge value=""{kiRecharge}"" />
                    <I_32 value=""1"" />
                    <I_36 value=""1"" />
                    <I_40 value=""0"" />
                    <Stamina value=""{stamina}"" />
                    <Stamina_Recharge value=""{staminaRecharge}"" />
                    <F_52 value=""1.0"" />
                    <F_56 value=""1.1"" />
                    <I_60 value=""0"" />
                    <Basic_Atk_Defense value=""{basicAtkDefense}"" />
                    <Basic_Ki_Defense value=""{basicKiDefense}"" />
                    <Strike_Atk_Defense value=""{strikeAtkDefense}"" />
                    <Super_Ki_Defense value=""{superKiDefense}"" />
                    <Ground_Speed value=""{groundSpeed}"" />
                    <Air_Speed value=""{airSpeed}"" />
                    <Boost_Speed value=""{boostingSpeed}"" />
                    <Dash_Speed value=""{dashSpeed}"" />
                    <F_96 value=""1.0"" />
                    <Reinforcement_Skill_Duration value=""{reinfSkillDuration}"" />
                    <F_104 value=""1.0"" />
                    <Revival_HP_Amount value=""{revivalHpAmount}"" />
                    <Reviving_Speed value=""{revivingSpeed}"" />
                    <F_116 value=""1.0"" />
                    <F_120 value=""0.55"" />
                    <F_124 value=""1.0"" />
                    <F_128 value=""1.0"" />
                    <F_132 value=""1.0"" />
                    <F_136 value=""1.0"" />
                    <I_140 value=""0"" />
                    <F_144 value=""1.0"" />
                    <F_148 value=""1.0"" />
                    <F_152 value=""1.0"" />
                    <F_156 value=""1.0"" />
                    <F_160 value=""1.0"" />
                    <F_164 value=""1.0"" />
                    <Z-Soul value=""{zSoul}"" />
                    <I_172 value=""1"" />
                    <I_176 value=""1"" />
                    <F_180 value=""8.0"" />
                  </PscSpecEntry>
                </PSC_Entry>
              </Configuration>
            </PSC>");
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
                        text10.Append(s.Replace("{[JCO,0,0,0,110,111]}", "{[JCO,0,0,0,110,111]}{[" + entryName + $",0,0,0,{-1},{-1}]}}"));
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
                    expand[expand.Length - 1].Lines = new string[] { charaNameEn };
                    expand[expand.Length - 1].NameID = "chara_" + entryName + "_" + (endid).ToString("000");

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
                    if (!Directory.Exists(Settings.Default.datafolder + @"/chara/"))
                        Directory.CreateDirectory(Settings.Default.datafolder + @"/chara/");
                    Directory.Move(Settings.Default.datafolder + "/" + entryName, Settings.Default.datafolder + @"/chara/" + entryName);

                    string embpackPath = Path.Combine(Settings.Default.datafolder, @"ui\texture", "embpack.exe");

                    File.Move(Settings.Default.datafolder + @"/ui/SEL.DDS", Settings.Default.datafolder + $"/ui/texture/CHARA01/{entryName}_000.DDS");
                    if (Directory.Exists(Settings.Default.datafolder + @"/JUNGLE/data"))
                    {
                        MergeDirectoriesWithConfirmation(Settings.Default.datafolder + @"/JUNGLE/data", Settings.Default.datafolder);
                        Directory.Delete(Settings.Default.datafolder + @"/JUNGLE", true);
                    }
                    if (Directory.Exists(Settings.Default.datafolder + @"/SKILL_ATACHMENT"))
                        Directory.Delete(Settings.Default.datafolder + @"/SKILL_ATACHMENT", true);
                    

                    ConvertCharaEMB(entryName);
                    RunCommand($"\"{embpackPath}\" \"{Settings.Default.datafolder}\\ui\\texture\\CHARA01\"");
                    string[] row = { charaNameEn, modAuthor, "Added Character" };
                    ListViewItem lvi = new ListViewItem(row);
                    lvMods.Items.Add(lvi);
                    saveLvItems();
                    Clean();
                }
            }
        }
        private void ConvertCharaEMB(string entryName)
        {

            string extractFolder = Path.Combine(Properties.Settings.Default.datafolder, @"ui\texture");

            string embpackPath = Path.Combine(extractFolder, "embpack.exe");
            string ddsFolder = Path.Combine(Settings.Default.datafolder, "chara", entryName);
            string[] embFiles = Directory.GetFiles(ddsFolder, "*.dyt.emb", SearchOption.AllDirectories);
            Console.WriteLine($"Found {embFiles.Length} EMB files in {ddsFolder}");


            // Check if the folder exists
            if (!Directory.Exists(ddsFolder))
            {
                Console.WriteLine($"Folder not found: {ddsFolder}");
                return;
            }
            foreach (string embfile in embFiles)
            {
                RunCommand($"\"{embpackPath}\" \"{embfile}\"");
            }

            // Get all DDS files in the "chara/{entryName}" directory
            string[] ddsFiles = Directory.GetFiles(ddsFolder, "*.dds", SearchOption.AllDirectories);

            if (ddsFiles.Length == 0)
            {
                Console.WriteLine("No DDS files found in the specified folder.");
                return;
            }

            // Group DDS files by their parent folder to process them together
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

            // Process each group of DDS files in the same folder
            foreach (var group in groupedByFolder)
            {
                string folder = group.Key;
                List<string> ddsFilesInFolder = group.Value;

                Console.WriteLine($"Processing folder: {folder}");

                // Clean each DDS file in the folder
                foreach (string ddsFile in ddsFilesInFolder)
                {
                    Console.WriteLine($"Cleaning DDS file: {ddsFile}");
                    DDS.CleanDDSForXV1(ddsFile, ddsFile);
                }

                // Create the corresponding .emb file path by replacing the .dds extension with .emb
                // Prepend entryName to the emb file name

                // Ensure the directory for the .emb file exists
                if (!Directory.Exists(folder))
                {
                    Console.WriteLine($"Directory does not exist for embFilePath: {folder}");
                    continue;
                }

                // Run the packing command to pack the entire folder back into the .emb file
                RunCommand($"\"{embpackPath}\" \"{folder}\"");
                Directory.Delete(folder, true);
            }



        }
        private void convertAllModelsToXV1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ProcessFiles(Settings.Default.datafolder + @"/chara");
            foreach(string entryName in Directory.GetDirectories(Settings.Default.datafolder + @"/chara"))
                ConvertCharaEMB(entryName);
            MessageBox.Show("Done!", "Success", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void convertDDSToXV1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "*.dds|*.dds";

            if (ofd.ShowDialog() == DialogResult.OK)
                DDS.CleanDDSForXV1(ofd.FileName, ofd.FileName);
        }

        private void removeEmptyEntriesToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CharSkill skill = new CharSkill();
            skill.populateSkillData(Settings.Default.datafolder + @"/msg", Settings.Default.datafolder + @"/system/custom_skill.cus", language);
            skill.RemoveInvalidEntriesAndReplace();
            skill.Save();

        }
    }

}