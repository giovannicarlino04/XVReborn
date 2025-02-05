using System;
using System.IO;

namespace XVReborn
{
    internal class DDS
    {
        public static void CleanDDSForXV1(string inputPath, string outputPath)
        {
            byte[] ddsData = File.ReadAllBytes(inputPath);
            int blockSize = DetectDXTFormat(ddsData);
            if (blockSize == 0)
            {
                Console.WriteLine("❌ Unsupported DDS format. Only DXT1, DXT3, and DXT5 are supported.");
                return;
            }

            (uint x, uint y, uint w, uint h)[] regions = {
                (0, 4, 128, 4),
                (0, 20, 128, 4),
                (0, 36, 128, 4),
                (0, 52, 128, 4),
                (0, 68, 128, 4),
                (0, 84, 128, 4),
                (0, 100, 128, 4)
            };

            foreach (var (startX, startY, regionWidth, regionHeight) in regions)
            {
                for (uint row = startY; row < startY + regionHeight; row++)
                {
                    for (uint col = startX; col < startX + regionWidth; col++)
                    {
                        uint blockX = col / 4;
                        uint blockY = row / 4;
                        uint blockOffset = CalculateBlockOffset(blockX, blockY, blockSize);
                        ModifyBlockToBlack(ref ddsData, blockOffset, blockSize);
                    }
                }
            }

            File.WriteAllBytes(outputPath, ddsData);
            Console.WriteLine($"✅ DDS file modified and saved as: {outputPath}");
        }

        private static int DetectDXTFormat(byte[] ddsData)
        {
            const int DXT1 = 0x31545844; // "DXT1"
            const int DXT3 = 0x33545844; // "DXT3"
            const int DXT5 = 0x35545844; // "DXT5"
            int format = BitConverter.ToInt32(ddsData, 84);
            switch (format)
            {
                case DXT1: return 8;
                case DXT3: return 16;
                case DXT5: return 16; // DXT5 uses 16 bytes per block like DXT3
                default: return 0;
            }
        }

        private static uint CalculateBlockOffset(uint blockX, uint blockY, int blockSize)
        {
            uint widthInBlocks = 128 / 4;
            return (blockY * widthInBlocks + blockX) * (uint)blockSize + 128;
        }

        private static void ModifyBlockToBlack(ref byte[] ddsData, uint blockOffset, int blockSize)
        {
            if (blockSize == 8)
            {
                byte[] blackBlockDXT1 = new byte[8] { 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00 };
                Array.Copy(blackBlockDXT1, 0, ddsData, blockOffset, 8);
            }
            else if (blockSize == 16)
            {
                byte[] blackBlockDXT3 = new byte[16] {
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, // Alpha values (all 0)
                    0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00, 0x00  // Color data (black)
                };
                Array.Copy(blackBlockDXT3, 0, ddsData, blockOffset, 16);
            }
            else if (blockSize == 16) // Support for DXT5 as well
            {
                byte[] blackBlockDXT5 = new byte[16] {
                    0x00, 0x00, 0x00, 0x00,  // Alpha ramp (fully transparent)
                    0x00, 0x00, 0x00, 0x00,  // Alpha ramp (fully transparent)
                    0x00, 0x00, 0x00, 0x00,  // Color data (black)
                    0x00, 0x00, 0x00, 0x00   // Color data (black)
                };
                Array.Copy(blackBlockDXT5, 0, ddsData, blockOffset, 16);
            }
        }
    }
}
