using System;
using System.Collections.Generic;
using System.Text;

namespace ImageToTextArt.BmpLoader
{
    public struct BmpHeader
    {
        public UInt16 Signature;
        public UInt32 FileSize;
        public UInt32 Reserved;
        public UInt32 DataOffSet;

    }

    public struct BmpInfoHeader
    {
        public UInt32 Size;
        public UInt32 Width;
        public UInt32 Height;
        public UInt16 Planes;
        public UInt16 BitPerPixel;
        public UInt32 Compression;
        public UInt32 ImageSize;
        public UInt32 XpixelsPerM;
        public UInt32 YpixelsPerM;
        public UInt32 ColorsUsed;
        public UInt32 ImportantColors;
    }

    public struct BmpPixel
    {
        public byte Red;
        public byte Green;
        public byte Blue;
        public byte Alpha;
    }
       
}
