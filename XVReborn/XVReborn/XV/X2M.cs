using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using XVReborn.Properties;

namespace XVReborn.XV
{
    public class X2M
    {
        public void ProcessBCS(string filePath)
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

        public void ChangeModelVer(string filePath)
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
        public void ChangeShaderHeader(string filePath)
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
        public void ChangeShaderVer(string filePath)
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
        public void ChangeImageVer(string filePath)
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
        public void ProcessBAC(string filePath)
        {
            Process p = new Process();
            ProcessStartInfo info = new ProcessStartInfo();
            info.FileName = "cmd.exe";
            info.CreateNoWindow = true;
            info.WindowStyle = ProcessWindowStyle.Hidden;
            info.RedirectStandardInput = true;
            info.RedirectStandardOutput = true;
            info.UseShellExecute = false;
            p.StartInfo = info;
            p.Start();

            if (filePath.EndsWith(".bdm"))
            {
                using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.ReadWrite))
                using (var reader = new BinaryReader(stream))
                using (var writer = new BinaryWriter(stream))
                {
                    stream.Seek(0, SeekOrigin.Begin);

                    byte[] byteArray = new byte[]
                    {
                0x23, 0x42, 0x44, 0x4D, 0xFE, 0xFF, 0x00, 0x00, 0x05, 0x01,
                0x00, 0x00, 0x10, 0x00, 0x00, 0x00, 0x96, 0x01, 0x64, 0x00,
                0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00,
                0xFF, 0xFF
                    };

                    writer.Write(byteArray);

                    Console.WriteLine($"Processed {filePath}");
                }
            }

            Console.WriteLine($"Processed {filePath} (BAC/BDM)");
        }

    }
}
