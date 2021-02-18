using System;
using System.Collections.Generic;
using System.Text;

namespace ImageToTextArt
{
    public static class HelperFunctions
    {
        public static int Sum(byte[,] pixels)
        {
            var total = 0;
            foreach (var p in pixels)
            {
                total += p;
            }
            return total;

        }

    }
}
