using System;
using System.Collections.Generic;
using System.Text;

namespace ImageToTextArt
{
    class Matchers
    {

        public static char FindBestMatch(byte[,] window, List<(byte[,] bmp, char chr)> charImages)
        {
            return SumMatch(window, charImages);
            return AverageDifferenceMatcher(window, charImages);
        }

        static char AverageDifferenceMatcher(byte[,] window, List<(byte[,] bmp, char chr)> charImages)
        {
            var diff = 100000000000000;
            
            char c = 'a';
            foreach (var charImg in charImages)
            {
                var ad = AverageDiff(window, charImg.bmp);

                if ( ad < diff)
                {
                    diff = ad;
                    c = charImg.chr;
                }
            }

            return c;
        }

        static int AverageDiff(byte[,] window, byte[,] charImg)
        {
            var height = window.GetLength(0);
            var width = window.GetLength(1);

            var totalDiff = 0;
            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    totalDiff += Math.Abs(window[y, x] - charImg[y, x]);
                }
            }

            return totalDiff / (width * height);
        }


        static char SumMatch(byte[,] window, List<(byte[,] bmp, char chr)> charImages)
        {
            var wSum = HelperFunctions.Sum(window);
            var diff = 100000000000000;

            char c = 'a';
            foreach (var charImg in charImages)
            {
                var cSum = HelperFunctions.Sum(charImg.bmp);

                if (Math.Abs(cSum - wSum) < diff)
                {
                    diff = Math.Abs(cSum - wSum);
                    c = charImg.chr;
                }
            }

            return c;
        }


        public class Img
        {
            public Img()
            {

            }

            public Img(byte[,] pixels)
            {
                Pixels = pixels;
            }

            public byte[,] Pixels;

            public int Width => Pixels.GetLength(1);

            public int Height => Pixels.GetLength(0);


            public static implicit operator byte[,](Img i) => i.Pixels;
            public static explicit operator Img(byte[,] pxs) => new Img(pxs);

            public static Img operator -(Img a, Img b)
            {
                byte[,] pixels = new byte[a.Height, a.Width];

                for (var y = 0; y < a.Height; y++)
                {
                    for (var x = 0; x < a.Width; x++)
                    {
                        pixels[y, x] = (byte)(((byte[,])a)[y, x] - ((byte[,])b)[y, x]);

                    }
                }
                return new Img(pixels);
            }

        }

       
    }
}
