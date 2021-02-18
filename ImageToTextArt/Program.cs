using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using ImageToTextArt.BmpLoader;

namespace ImageToTextArt
{
    class Program
    {
        static void Main(string[] args)
        {
            var charBmps = CharBitMapGenerator.GenerateCharBitMapImages();

            var grayScaleBmps = charBmps.Select(x => (BmpToGrayScalePixels(x.bmp), x.chr)).ToList();

            var img = Image.FromFile(@"C:\Users\PC\source\repos\ImageToTextArt\download.jpg");


             Bmp bmp;
            using(var ms = new MemoryStream())
            {
                img.Save(ms, ImageFormat.Bmp);
                bmp = BmpParser.Parse(ms.ToArray());
            }

            var grayscale = BmpToGrayScalePixels(bmp);

            var resString = ImageToText.GenerateImageString(grayscale, grayScaleBmps);

         }

        static byte[,] Inverse(byte[,] pixels)
        {
            var res = new byte[pixels.GetLength(0), pixels.GetLength(1)];
            for (var y = 0; y < pixels.GetLength(0); y++)
            {
                for (var x = 0; x < pixels.GetLength(1); x++)
                {
                    res[y, x] = (byte)(255 - pixels[y, x]);
                }
            }
            return res;
        }


        static byte[,] BmpToGrayScalePixels(Bmp bmp)
        {
            var res = new byte[bmp.Height, bmp.Width];
            for(var y = 0; y < bmp.Height; y++)
            {
                for (var x = 0; x < bmp.Width; x++)
                {
                    res[y, x] =(byte) ((0.3 * bmp.Pixels[y, x].Red) + (0.59 * bmp.Pixels[y, x].Green) + (0.11 * bmp.Pixels[y, x].Blue));
                }
            }
            return res;
        }

    }
}
