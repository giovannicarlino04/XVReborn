using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace XVReborn
{
    public struct skill
    {
        public string Name;
        public short ID;
    }

    public struct Char_Data
    {
        public int charID;
        public int CostumeID;
        public short[] SuperIDs;
        public short[] UltimateIDs;
        public short EvasiveID;
    }

    class CharSkill
    {
        string FileName;
        public skill[] Supers;
        public skill[] Ultimates;
        public skill[] Evasives;
        int CharCount = 0;
        int CharAddress = 0;
        public Char_Data[] Chars;
        msg mText = new msg();

        public void populateSkillData(string msgFolder, string CUSFile, string lang)
        {
            FileName = CUSFile;
            using (BinaryReader CUS = new BinaryReader(File.Open(CUSFile, FileMode.Open)))
            {
                CUS.BaseStream.Seek(8, SeekOrigin.Begin);
                CharCount = CUS.ReadInt32();
                CharAddress = CUS.ReadInt32();

                int SuperCount = CUS.ReadInt32();
                int UltimateCount = CUS.ReadInt32();
                int EvasiveCount = CUS.ReadInt32();

                CUS.BaseStream.Seek(8, SeekOrigin.Current);

                int SupAddress = CUS.ReadInt32();
                int UltAddress = CUS.ReadInt32();
                int EvaAddress = CUS.ReadInt32();


                Chars = new Char_Data[CharCount];
                for (int i = 0; i < CharCount; i++)
                {
                    CUS.BaseStream.Seek(CharAddress + (i * 32), SeekOrigin.Begin);
                    Chars[i].charID = CUS.ReadInt32();
                    Chars[i].CostumeID = CUS.ReadInt32();

                    Chars[i].SuperIDs = new short[4];
                    Chars[i].UltimateIDs = new short[2];

                    Chars[i].SuperIDs[0] = CUS.ReadInt16();
                    Chars[i].SuperIDs[1] = CUS.ReadInt16();
                    Chars[i].SuperIDs[2] = CUS.ReadInt16();
                    Chars[i].SuperIDs[3] = CUS.ReadInt16();
                    Chars[i].UltimateIDs[0] = CUS.ReadInt16();
                    Chars[i].UltimateIDs[1] = CUS.ReadInt16();
                    Chars[i].EvasiveID = CUS.ReadInt16();
                }

                Supers = new skill[SuperCount];
                mText = msgStream.Load(msgFolder + "/proper_noun_skill_spa_name_" + lang + ".msg");
                for (int i = 0; i < SuperCount; i++)
                {
                    CUS.BaseStream.Seek(SupAddress + (i * 48) + 8, SeekOrigin.Begin);
                    Supers[i].ID = CUS.ReadInt16();
                    Supers[i].Name = findName("spe_skill_" + CUS.ReadInt16().ToString("000"));
                }

                Ultimates = new skill[UltimateCount];
                mText = msgStream.Load(msgFolder + "/proper_noun_skill_ult_name_" + lang + ".msg");
                for (int i = 0; i < UltimateCount; i++)
                {
                    CUS.BaseStream.Seek(UltAddress + (i * 48) + 8, SeekOrigin.Begin);
                    Ultimates[i].ID = CUS.ReadInt16();
                    Ultimates[i].Name = findName("ult_" + CUS.ReadInt16().ToString("000"));
                }

                Evasives = new skill[EvasiveCount];
                mText = msgStream.Load(msgFolder + "/proper_noun_skill_esc_name_" + lang + ".msg");
                for (int i = 0; i < EvasiveCount; i++)
                {
                    CUS.BaseStream.Seek(EvaAddress + (i * 48) + 8, SeekOrigin.Begin);
                    Evasives[i].ID = CUS.ReadInt16();
                    Evasives[i].Name = findName("avoid_skill_" + CUS.ReadInt16().ToString("000"));
                }

            }
        }

        public void RemoveInvalidEntriesAndReplace()
        {
            // Step 1: Read all the data into memory
            List<Char_Data> validChars = new List<Char_Data>();
            int charCount;
            string fileName = FileName;

            // Read the data into memory
            using (BinaryReader reader = new BinaryReader(File.Open(fileName, FileMode.Open, FileAccess.Read)))
            {
                reader.BaseStream.Seek(8, SeekOrigin.Begin);
                charCount = reader.ReadInt32();
                int charAddress = reader.ReadInt32();

                // Read the counts of each skill type
                int superCount = reader.ReadInt32();
                int ultimateCount = reader.ReadInt32();
                int evasiveCount = reader.ReadInt32();

                reader.BaseStream.Seek(8, SeekOrigin.Current); // Skip over padding

                int superAddress = reader.ReadInt32();
                int ultimateAddress = reader.ReadInt32();
                int evasiveAddress = reader.ReadInt32();

                // Process each character
                for (int i = 0; i < charCount; i++)
                {
                    reader.BaseStream.Seek(charAddress + (i * 32), SeekOrigin.Begin);
                    Char_Data character = new Char_Data
                    {
                        charID = reader.ReadInt32(),
                        CostumeID = reader.ReadInt32(),
                        SuperIDs = new short[4],
                        UltimateIDs = new short[2]
                    };

                    // Read the Super, Ultimate, and Evasive IDs for the character
                    for (int j = 0; j < 4; j++)
                        character.SuperIDs[j] = reader.ReadInt16();
                    for (int j = 0; j < 2; j++)
                        character.UltimateIDs[j] = reader.ReadInt16();
                    character.EvasiveID = reader.ReadInt16();

                    // Validate skills for the character
                    var validSupers = GetValidSuperSkills();
                    var validUltimates = GetValidUltimateSkills();
                    var validEvasives = GetValidEvasiveSkills();

                    // Check and replace invalid skill entries
                    for (int j = 0; j < 4; j++)
                    {
                        if (!validSupers.Contains(character.SuperIDs[j]))
                        {
                            character.SuperIDs[j] = -1; // Replace with 65535 if invalid
                        }
                    }

                    for (int j = 0; j < 2; j++)
                    {
                        if (!validUltimates.Contains(character.UltimateIDs[j]))
                        {
                            character.UltimateIDs[j] = -1; // Replace with 65535 if invalid
                        }
                    }

                    if (!validEvasives.Contains(character.EvasiveID))
                    {
                        character.EvasiveID = -1; // Replace with 65535 if invalid
                    }

                    // Add the character to the validChars list (even if they have invalid skills, but corrected)
                    validChars.Add(character);
                }
            } // BinaryReader is now closed

            // Step 2: Write the modified data back to the file
            using (BinaryWriter writer = new BinaryWriter(File.Open(fileName, FileMode.Open, FileAccess.Write)))
            {
                // Update the character count and position in the CUS file
                writer.BaseStream.Seek(8, SeekOrigin.Begin);
                writer.Write(validChars.Count); // Write the new valid character count

                // Write the character data to the file
                int charAddress = 0x30; // Set this to the address where the character data starts (adjust if needed)
                writer.BaseStream.Seek(charAddress, SeekOrigin.Begin);
                foreach (var character in validChars)
                {
                    writer.Write(character.charID);
                    writer.Write(character.CostumeID);
                    foreach (var superID in character.SuperIDs)
                        writer.Write(superID);
                    foreach (var ultimateID in character.UltimateIDs)
                        writer.Write(ultimateID);
                    writer.Write(character.EvasiveID);
                }
            } // BinaryWriter is now closed

            Console.WriteLine($"{charCount - validChars.Count} invalid skill entries were replaced with 65535.");
        }

        // Aggiungi questi metodi per ottenere le skill valide
        public List<short> GetValidSuperSkills()
        {
            return Supers.Select(s => s.ID).ToList();
        }

        public List<short> GetValidUltimateSkills()
        {
            return Ultimates.Select(u => u.ID).ToList();
        }

        public List<short> GetValidEvasiveSkills()
        {
            return Evasives.Select(e => e.ID).ToList();
        }
        public void AddCharacter(Char_Data newChar)
        {
            // Controlla se il personaggio esiste già
            if (DataExist(newChar.charID, newChar.CostumeID) != -1)
            {
                Console.WriteLine("Character already exists!");
                return;
            }

            // Aggiungi il nuovo personaggio all'array ridimensionandolo
            Array.Resize(ref Chars, Chars.Length + 1);
            Chars[Chars.Length - 1] = newChar;
            CharCount++;

            // Ottieni le skill valide per il personaggio
            var validSupers = GetValidSuperSkills();
            var validUltimates = GetValidUltimateSkills();
            var validEvasives = GetValidEvasiveSkills();

            newChar.SuperIDs[0] = IsValidSkill(newChar.SuperIDs[0].ToString(), validSupers);
            newChar.SuperIDs[1] = IsValidSkill(newChar.SuperIDs[1].ToString(), validSupers);
            newChar.SuperIDs[2] = IsValidSkill(newChar.SuperIDs[2].ToString(), validSupers);
            newChar.SuperIDs[3] = IsValidSkill(newChar.SuperIDs[3].ToString(), validSupers);

            newChar.UltimateIDs[0] = IsValidSkill(newChar.UltimateIDs[0].ToString(), validUltimates);
            newChar.UltimateIDs[1] = IsValidSkill(newChar.UltimateIDs[1].ToString(), validUltimates);

            newChar.EvasiveID = IsValidSkill(newChar.EvasiveID.ToString(), validEvasives);


            // Scrive il nuovo personaggio nel file binario
            using (BinaryWriter CUS = new BinaryWriter(File.Open(FileName, FileMode.Open)))
            {
                // Aggiorna il contatore dei personaggi
                CUS.BaseStream.Seek(8, SeekOrigin.Begin);
                CUS.Write(CharCount);

                // Scrive il nuovo personaggio alla fine della lista
                int newOffset = CharAddress + ((CharCount - 1) * 32);
                CUS.BaseStream.Seek(newOffset, SeekOrigin.Begin);
                CUS.Write(newChar.charID);
                CUS.Write(newChar.CostumeID);
                CUS.Write(newChar.SuperIDs[0]);
                CUS.Write(newChar.SuperIDs[1]);
                CUS.Write(newChar.SuperIDs[2]);
                CUS.Write(newChar.SuperIDs[3]);
                CUS.Write(newChar.UltimateIDs[0]);
                CUS.Write(newChar.UltimateIDs[1]);
                CUS.Write(newChar.EvasiveID);
            }

            Console.WriteLine("Character added successfully.");
        }
        private short IsValidSkill(string skillId, List<short> validSkills)
        {
            if (short.TryParse(skillId, out short skill) && validSkills.Contains(skill) && skill != 65535 && skill != 0)
            {
                return skill; 
            }
            return -1; 
        }

        public void Save()
        {
            using (BinaryWriter CUS = new BinaryWriter(File.Open(FileName, FileMode.Open)))
            {
                CUS.BaseStream.Seek(CharAddress, SeekOrigin.Begin);
                for (int i = 0; i < CharCount; i++)
                {
                    CUS.BaseStream.Seek(CharAddress + (i * 32) + 8, SeekOrigin.Begin);
                    CUS.Write(Chars[i].SuperIDs[0]);
                    CUS.Write(Chars[i].SuperIDs[1]);
                    CUS.Write(Chars[i].SuperIDs[2]);
                    CUS.Write(Chars[i].SuperIDs[3]);
                    CUS.Write(Chars[i].UltimateIDs[0]);
                    CUS.Write(Chars[i].UltimateIDs[1]);
                    CUS.Write(Chars[i].EvasiveID);
                }
            }
        }

        private string findName(string text_ID)
        {
            for (int i = 0; i < mText.data.Length; i++)
            {
                if (mText.data[i].NameID == text_ID)
                    return mText.data[i].Lines[0];
            }


            return "Unknown Skill";
        }

        public int FindSuper(short id)
        {
            for (int i = 0; i < Supers.Length; i++)
            {
                if (Supers[i].ID == id)
                    return i;
            }
            return -1;
        }

        public int FindUltimate(short id)
        {
            for (int i = 0; i < Ultimates.Length; i++)
            {
                if (Ultimates[i].ID == id)
                    return i;
            }
            return -1;
        }

        public int FindEvasive(short id)
        {
            for (int i = 0; i < Evasives.Length; i++)
            {
                if (Evasives[i].ID == id)
                    return i;
            }
            return -1;
        }

        public int DataExist(int id, int c)
        {
            for (int i = 0; i < Chars.Length; i++)
            {
                if (Chars[i].charID == id && Chars[i].CostumeID == c)
                    return i;
            }

            return -1;
        }
    }

}
