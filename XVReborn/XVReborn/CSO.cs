﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace XVReborn
{
    public struct CSO_Data
    {
        public int Char_ID;
        public int Costume_ID;
        public string[] Paths;
    }

    public class CSO
    {
        public CSO_Data[] Data;
        BinaryReader br;
        BinaryWriter bw;
        string FileName;

        public void Load(string path)
        {
            using (br = new BinaryReader(File.Open(path, FileMode.Open)))
            {
                FileName = path;
                br.BaseStream.Seek(8, SeekOrigin.Begin);
                int Count = br.ReadInt32();
                Data = new CSO_Data[Count];
                int Offset = br.ReadInt32();

                for (int i = 0; i < Count; i++)
                {
                    br.BaseStream.Seek(Offset + (32 * i), SeekOrigin.Begin);
                    Data[i].Char_ID = br.ReadInt32();
                    Data[i].Costume_ID = br.ReadInt32();
                    Data[i].Paths = new string[4];
                    Data[i].Paths[0] = TextAtAddress(br.ReadInt32());
                    Data[i].Paths[1] = TextAtAddress(br.ReadInt32());
                    Data[i].Paths[2] = TextAtAddress(br.ReadInt32());
                    Data[i].Paths[3] = TextAtAddress(br.ReadInt32());
                }
            }
        }

        public string TextAtAddress(int Address)
        {
            long position = br.BaseStream.Position;
            string rText = "";
            byte[] c;
            if (Address != 0)
            {
                br.BaseStream.Seek(Address, SeekOrigin.Begin);
                do
                {
                    c = br.ReadBytes(1);
                    if (c[0] != 0x00)
                        rText += System.Text.Encoding.ASCII.GetString(c);
                    else
                        break;
                }
                while (true);

                br.BaseStream.Seek(position, SeekOrigin.Begin);
            }
            return rText;
        }

        public void Save()
        {
            List<string> CmnText = new List<string>();
            for (int i = 0; i < Data.Length; i++)
            {
                for (int j = 0; j < Data[i].Paths.Length; j++)
                {
                    if (!CmnText.Contains(Data[i].Paths[j]))
                        CmnText.Add(Data[i].Paths[j]);
                }
            }

            int[] wordAddress = new int[CmnText.Count];
            int wordstartposition = 16 + (Data.Length * 32);
            using (bw = new BinaryWriter(File.Create(FileName)))
            {
                bw.Write(new byte[] { 0x23, 0x43, 0x53, 0x4F, 0xFE, 0xFF, 0x00, 0x00 });
                bw.Write(Data.Length);
                bw.Write((int)16);
                bw.Seek(wordstartposition, SeekOrigin.Begin);
                for (int i = 0; i < CmnText.Count; i++)
                {
                    wordAddress[i] = (int)bw.BaseStream.Position;
                    bw.Write(System.Text.Encoding.ASCII.GetBytes(CmnText[i]));
                    bw.Write((byte)0);
                }

                for (int i = 0; i < Data.Length; i++)
                {
                    bw.BaseStream.Seek(16 + (32 * i), SeekOrigin.Begin);
                    bw.Write(Data[i].Char_ID);
                    bw.Write(Data[i].Costume_ID);
                    bw.Write(wordAddress[CmnText.IndexOf(Data[i].Paths[0])]);
                    bw.Write(wordAddress[CmnText.IndexOf(Data[i].Paths[1])]);
                    bw.Write(wordAddress[CmnText.IndexOf(Data[i].Paths[2])]);
                    bw.Write(wordAddress[CmnText.IndexOf(Data[i].Paths[3])]);
                }
            }
        }

        public int DataExist(int id, int c)
        {
            for (int i = 0; i < Data.Length; i++)
            {
                if (Data[i].Char_ID == id && Data[i].Costume_ID == c)
                    return i;
            }

            for (int j = 0; j < Data.Length; j++)
            {
                if (Data[j].Char_ID == id)
                    return j;
            }

            return -1;
        }
        public void AddCharacter(CSO_Data character)
        {
            int existingIndex = DataExist(character.Char_ID, character.Costume_ID);

            if (existingIndex >= 0)
            {
                Data[existingIndex] = character;
            }
            else
            {
                List<CSO_Data> newData = Data.ToList();
                newData.Add(character);
                Data = newData.ToArray();
            }

            Save();
        }

    }
}
