using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;

namespace ImageToTextArt
{
    class ImageToText
    {

        public static string GenerateImageString(byte[,] grayScapeInput, List<(byte[,] bmp, char chr)> charImages)
        {

            var imgHeight = grayScapeInput.GetLength(0);
            var imgWidth = grayScapeInput.GetLength(1);



            var winHeight = charImages[0].bmp.GetLength(0);
            var winWidth = charImages[0].bmp.GetLength(1);

            var res = "";
            for(var y = 0; y < imgHeight; y += winHeight)
            {
                for (var x = 0; x < imgWidth; x += winWidth)
                {
                    var window = CreateWindow(grayScapeInput, new Rectangle(x, y, winWidth, winHeight));
                    var c = FindBestMatch(window, charImages);
                    res += c;
                }
                res += "\n";
            }

            // pad image to match charImages size

            File.WriteAllText(@"C:\Users\PC\source\repos\ImageToTextArt\out.txt", res);
            // go over each windows and find best matching char
            return res;
            

        }


        static char FindBestMatch(byte[,] window, List<(byte[,] bmp, char chr)> charImages)
        {
            return SumMatch(window, charImages);    
        }


        static char SumMatch(byte[,] window, List<(byte[,] bmp, char chr)> charImages)
        {
            var wSum = Sum(window);
            var diff = 100000000000000;

            char c = 'a';
            foreach(var charImg in charImages)
            {
                var cSum = Sum(charImg.bmp);

                if(Math.Abs(cSum - wSum) < diff)
                {
                    diff = Math.Abs(cSum - wSum);
                    c = charImg.chr;
                }                
            }

            return c;
        }

        static int Sum(byte[,] pixels)
        {
            var total = 0;
            foreach(var p in pixels)
            {
                total += p;
            }
            return total;

        }

        static byte[,] CreateWindow(byte[,] grayScapeInput, Rectangle rect)
        {
            var window = new byte[rect.Height, rect.Width];
            for (var y = 0; y < rect.Height; y++)
            {
                for (var x = 0; x < rect.Width; x++)
                {
                    if (y + rect.Y < grayScapeInput.GetLength(0) && x + rect.X < grayScapeInput.GetLength(1))
                    {
                        window[y, x] = grayScapeInput[y + rect.Y, x + rect.X];
                    }
                    else
                    {
                        window[y, x] = 0;
                    }
                }
            }


            return window;
        }
    }
}
