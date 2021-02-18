using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using ImageToTextArt.BmpLoader;

namespace ImageToTextArt
{
    class CharBitMapGenerator
    {
        public static List<(Bmp bmp, char chr)> GenerateCharBitMapImages()
        {
            var bmps = new List<Bmp>();
            char start = (char)32;
            var maxChar = 128;
            for (char c = start; c < maxChar; c++)
            {
                bmps.Add(CharBmp(c));
            }

            var allRect = bmps.Select(x => GetRectangle(x)).ToList();

            var rects = allRect.Where(x => !x.IsEmpty).ToList();

            var centrs = allRect.Select(x => ((x.Right + x.Left) / 2, (x.Y = x.Bottom) / 2)).ToList();
            
            var x = rects.Min(x => x.X);
            var y = rects.Min(x => x.Y);
            var right = rects.Max(x => x.Right);
            var bottom = rects.Max(x => x.Bottom);


            var hasMax = allRect.Zip(Enumerable.Range(start, allRect.Count()), (r, i) => (r, i)).Where(d => d.r.Width == 23).ToList();

            var rect = new Rectangle(x, y, right , bottom);
            for (char c = start; c < maxChar; c++)
            {
                var (resized, valid) = ShrinkToRect(bmps[c - start], rect);

                if(valid)
                {
                    var name = c.ToString().Length >101 ? c.ToString() : ((int)c).ToString();
                }
            }

            return bmps.Select(bmp => ShrinkToRect(bmp, rect)).Where(x => x.valid).Select(x => x.bmp)
                .Zip(Enumerable.Range(start, bmps.Count()).Select(x => (char)x)).ToList();

        }

        static Bmp Binary(Bmp bmp)
        {
            for (var y = 0; y < bmp.Height; y++)
            {
                for (var x = 0; x < bmp.Width; x++)
                {
                    var pixel = bmp.Pixels[y, x];

                    if(NonBlack(pixel))
                    {
                        bmp.Pixels[y, x] = new BmpPixel { Alpha = 255, Blue = 255, Green = 255, Red = 255 };
                    }
                }
            }
            return bmp;
        }

        static Bmp SetAll(Bmp bmp, BmpPixel px)
        {
            for (var y = 0; y < bmp.Height; y++)
            {
                for (var x = 0; x < bmp.Width; x++)
                {
                    bmp.Pixels[y, x] = px;                    
                }
            }
            return bmp;
        }

        static Bmp CharBmp(char c)
        {
            var font = new Font("Consolas", 16f);

            var bitmap = new Bitmap(100,100);
        
            
            var s = c.ToString();
            var g = Graphics.FromImage(bitmap);

            g.DrawString(s, font, new SolidBrush(Color.White), 0, 0);
            g.Save();


            Bmp bmp;
            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms, ImageFormat.Bmp);
                bmp = BmpParser.Parse(ms.ToArray());
            }

            return Binary(bmp); 
        }

        static (Bmp bmp,bool valid) ShrinkToRect(Bmp bmp, Rectangle rect)
        {

            var newPixels = new BmpPixel[rect.Height, rect.Width];
            for (var y = rect.Y; y < rect.Bottom; y++)
            {
                for (var x = rect.X; x < rect.Right; x++)
                {
                    newPixels[y - rect.Y , x - rect.X] = bmp.Pixels[y, x];
                }
            }


            return (BmpParser.FromPixels(newPixels), newPixels.Length != 0);
        }

        static Rectangle GetRectangle(Bmp bmp)
        {

            var top = GetTop(bmp);
            var bottom = GetBottom(bmp);
            var left= GetLeft(bmp);
            var right = GetRight(bmp);
            
            return new Rectangle(left,top, right - left, bottom - top);
        }

        static Bmp DrawBox(Bmp bmp, Rectangle rect)
        {

            for (var y = rect.Y ; y < rect.Height + rect.Y; y++ )
            {
                bmp.Pixels[y, rect.X] = new BmpPixel { Alpha = 255, Green = 255, Blue = 255, Red = 255 };
                bmp.Pixels[y, rect.Right] = new BmpPixel { Alpha = 255, Green = 255, Blue = 255, Red = 255 };
            }

            for (var x = rect.X; x < rect.Width + rect.X; x++)
            {
                bmp.Pixels[rect.Y,x] = new BmpPixel { Alpha = 255, Green = 255, Blue = 255, Red = 255 };
                bmp.Pixels[rect.Bottom, x] = new BmpPixel { Alpha = 255, Green = 255, Blue = 255, Red = 255 };
            }

            return bmp;
        }

        static int GetTop(Bmp bmp)
        {
            for (var y = 0; y < bmp.InfoHeader.Height; y++)
            {
                for (var x = 0; x < bmp.InfoHeader.Width; x++)
                {
                    if (NonBlack(bmp.Pixels[y,x]))
                    {
                        return y;
                    }
                }
            }
            return 0;
        }

        static int GetBottom(Bmp bmp)
        {
            for (var y = bmp.Height -1; y >= 0; y--)
            {
                for (var x = 0; x < bmp.InfoHeader.Width; x++)
                {
                    if (NonBlack(bmp.Pixels[y, x]))
                    {
                        return y;
                    }
                }
            }
            
            return 0;
        }

        static int GetLeft(Bmp bmp)
        {
            for (var x = 0; x < bmp.InfoHeader.Width ; x++)
            {
                for (var y = 0; y < bmp.Height ; y++)
                 {
                
                    if (NonBlack(bmp.Pixels[y, x]))
                    {
                        if(x == 4)
                        {
                            var debug = 2;
                        }
                        return x;
                    }
                }
            }

            return 0;
        }

        static int GetRight(Bmp bmp)
        {
            for (var x = bmp.Width -1; x >= 0 ; x--)
            {
                for (var y = 0; y < bmp.Height ; y++)
                {

                    if (NonBlack(bmp.Pixels[y, x]))
                    {
                        return x;
                    }
                }
            }
            return 0;
        }
        
        static bool NonBlack(BmpPixel pixel )
        {
            return pixel.Red > 0 || pixel.Green > 0 || pixel.Blue > 0;

        }
        
    }
}
