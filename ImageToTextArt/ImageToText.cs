using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;


namespace ImageToTextArt
{
    class ImageToText
    {

        public static string GenerateImageString(byte[,] grayScapeInput, List<(byte[,] bmp, char chr)> charImages)
        {
            return OverAllIntensityBased(grayScapeInput, charImages);
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
                    var c = Matchers.FindBestMatch(window, charImages);
                    res += c;
                }
                res += "\n";
            }

            return res;
            

        }



        static string OverAllIntensityBased(byte[,] grayScapeInput, List<(byte[,] bmp, char chr)> charImages)
        {
            var rankedCharImages = charImages.OrderBy(x => HelperFunctions.Sum(x.bmp)).ToList(); ;

            var windows = CreateWindows(grayScapeInput, charImages).ToList();
            
            var sums = new (int sum, int index)[windows.Count];
            var i = 0;
            foreach(var w in windows)
            {
                sums[i] = (HelperFunctions.Sum(w.window), i);
                i++;
            }
            
            var max = sums.Max(x => x.sum);

            sums = sums.OrderBy(x => x.sum).ToArray();

            var scaled = sums.Select(x =>((byte) ( x.sum / (max / (rankedCharImages.Count-1.0d))),x.index)).ToArray();

            var max2 = scaled.Max(x => x.Item1);



            var imgHeight = grayScapeInput.GetLength(0);
            var imgWidth = grayScapeInput.GetLength(1);

            var winHeight = charImages[0].bmp.GetLength(0);
            var winWidth = charImages[0].bmp.GetLength(1);

            var res = "";
            i = 0;

            var ordered = scaled.OrderBy(x => x.index).ToArray();
            for (var y = 0; y < imgHeight; y += winHeight)
            {
                for (var x = 0; x < imgWidth; x += winWidth)
                {

                    res += rankedCharImages[ordered[i].Item1].chr.ToString();
                    i++;
                }
                res += "\n";
            }

            return res;
        }



        static IEnumerable<(byte[,] window, int index, int sum, int rank)> CreateWindows(byte[,] grayScapeInput, List<(byte[,] bmp, char chr)> charImages)
        {
            var imgHeight = grayScapeInput.GetLength(0);
            var imgWidth = grayScapeInput.GetLength(1);

            var winHeight = charImages[0].bmp.GetLength(0);
            var winWidth = charImages[0].bmp.GetLength(1);
            var index = 0;
            for (var y = 0; y < imgHeight; y += winHeight)
            {
                for (var x = 0; x < imgWidth; x += winWidth)
                {
                    yield return (CreateWindow(grayScapeInput, new Rectangle(x, y, winWidth, winHeight)), index,0,0);
                    index++;
                }
            }
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
