using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

namespace XVReborn
{
    internal class DDS
    {

        public static void CleanDDSForXV1(string inputPath, string outputPath)
        {
            // Apri il file DDS in modalità binaria
            byte[] ddsData = File.ReadAllBytes(inputPath);

            // Le regioni da modificare (x, y, larghezza, altezza)
            (uint x, uint y, uint w, uint h)[] regions = {
            (0, 4, 128, 4),
            (0, 20, 128, 4),
            (0, 36, 128, 4)
        };

            // Calcola la dimensione del blocco DXT1: ogni blocco di 4x4 pixel è 8 byte
            int blockSize = 8;

            // Iniziamo a lavorare direttamente con i byte per modificare i pixel neri
            foreach (var (startX, startY, regionWidth, regionHeight) in regions)
            {
                for (uint row = startY; row < startY + regionHeight; row++)
                {
                    for (uint col = startX; col < startX + regionWidth; col++)
                    {
                        // Calcola l'indice del blocco DXT1 a partire dalle coordinate pixel
                        uint blockX = col / 4;
                        uint blockY = row / 4;

                        // Calcola l'offset del blocco DXT1 nei dati del file
                        uint blockOffset = CalculateBlockOffset(blockX, blockY);

                        // Modifica i byte per il blocco corrente (sostituendolo con pixel neri)
                        ModifyBlockToBlack(ref ddsData, blockOffset);
                    }
                }
            }

            // Salva il file modificato
            File.WriteAllBytes(outputPath, ddsData);
            Console.WriteLine($"✅ DDS file modified and saved as: {outputPath}");
        }

        // Funzione per calcolare l'offset di un blocco nel file DXT1
        private static uint CalculateBlockOffset(uint blockX, uint blockY)
        {
            // Supponiamo che il file DDS sia un'immagine quadrata o rettangolare, calcoliamo l'offset
            uint widthInBlocks = 128 / 4; // Supponiamo larghezza immagine 128px
            uint heightInBlocks = 128 / 4; // Supponiamo altezza immagine 128px

            // Offset del blocco
            return (blockY * widthInBlocks + blockX) * 8 + 128; // 128 è l'offset dell'inizio dei dati d'immagine (header)
        }

        // Funzione per modificare un blocco DXT1 e impostarlo su nero
        private static void ModifyBlockToBlack(ref byte[] ddsData, uint blockOffset)
        {
            // Il formato DXT1 usa i colori a 5 bit per RGB e 1 bit per l'alpha
            // Il colore nero è (0, 0, 0) che in 5 bit per ogni canale diventa 0
            // Inoltre, dobbiamo configurare gli indici dei pixel per usare il colore nero

            byte[] blackBlock = new byte[8]
            {
            0x00, 0x00, // Colore 1: (0, 0, 0) in 5 bit per ciascun canale
            0x00, 0x00, // Colore 2: (0, 0, 0) in 5 bit per ciascun canale
            0x00, 0x00, // Indici dei pixel, tutti impostati su 0, quindi nero
            0x00, 0x00  // Indici dei pixel, tutti impostati su 0, quindi nero
            };

            // Copia i dati neri nel blocco specificato
            Array.Copy(blackBlock, 0, ddsData, blockOffset, 8);
        }
    }

}
