using System;
using System.Collections.Generic;
using System.Text;

namespace ImageToTextArt.BmpLoader
{
    public class Bmp
    {

        public Bmp()
        {

        }

        public BmpHeader Header;

        public BmpInfoHeader InfoHeader;

        public BmpPixel[,] Pixels;

        public int Height => (int)InfoHeader.Height;

        public int Width => (int)InfoHeader.Width;

        public byte[] GetFileBytes()
        {

            var res = new List<byte>();
            //HEADER
            res.AddRange(ByteIntConvertion.LoadBytesFromUInt16(Header.Signature));
            res.AddRange(ByteIntConvertion.LoadBytesFromUInt32(Header.FileSize));
            res.AddRange(ByteIntConvertion.LoadBytesFromUInt32(Header.Reserved));
            res.AddRange(ByteIntConvertion.LoadBytesFromUInt32(Header.DataOffSet));

            //INFOHEADER

            res.AddRange(ByteIntConvertion.LoadBytesFromUInt32(InfoHeader.Size));
            res.AddRange(ByteIntConvertion.LoadBytesFromUInt32(InfoHeader.Width));
            res.AddRange(ByteIntConvertion.LoadBytesFromUInt32(InfoHeader.Height));
            res.AddRange(ByteIntConvertion.LoadBytesFromUInt16(InfoHeader.Planes));
            res.AddRange(ByteIntConvertion.LoadBytesFromUInt16(InfoHeader.BitPerPixel));
            res.AddRange(ByteIntConvertion.LoadBytesFromUInt32(InfoHeader.Compression));
            res.AddRange(ByteIntConvertion.LoadBytesFromUInt32(InfoHeader.ImageSize));
            res.AddRange(ByteIntConvertion.LoadBytesFromUInt32(InfoHeader.XpixelsPerM));
            res.AddRange(ByteIntConvertion.LoadBytesFromUInt32(InfoHeader.YpixelsPerM));
            res.AddRange(ByteIntConvertion.LoadBytesFromUInt32(InfoHeader.ColorsUsed));
            res.AddRange(ByteIntConvertion.LoadBytesFromUInt32(InfoHeader.ImportantColors));


            //DATA

            for (var y = (int)InfoHeader.Height - 1; y >= 0; y--)
            {
                for (var x = 0;  x <  (int) InfoHeader.Width ; x++)
                {
                    res.Add(Pixels[y, x].Blue);
                    res.Add(Pixels[y, x].Green);
                    res.Add(Pixels[y, x].Red);

                    if (InfoHeader.BitPerPixel >= 32)
                    {
                        res.Add(Pixels[y, x].Alpha);
                    }
                }
                
            }
            return res.ToArray();

        }
    }
}
