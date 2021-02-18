using System;
using System.Collections.Generic;
using System.Text;

namespace ImageToTextArt.BmpLoader
{
    public class ByteIntConvertion
    {
        public static UInt32 LoadUInt32FromBytes(byte[] bmpBytes, int startOffSet)
        {
            var b1 = bmpBytes[startOffSet];

            var b2 = bmpBytes[startOffSet + 1] << 8;
            var b3 = bmpBytes[startOffSet + 2] << 16;
            var b4 = bmpBytes[startOffSet + 3] << 24;

            return (UInt32)(b1 + b2 + b3 + b4);
        }

        public static UInt16 LoadUInt16FromBytes(byte[] bmpBytes, int startOffSet)
        {
            var b2 = bmpBytes[startOffSet];
            var b1 = bmpBytes[startOffSet + 1] << 8;
            return (UInt16)(b1 + b2);
        }

        public static List<byte> LoadBytesFromUInt32(UInt32 data)
        {
            var res = new List<byte>();

            res.Add((byte)(data & 0xff));

            res.Add((byte)(data >> 8 & 0xff));

            res.Add((byte)(data >> 16 & 0xff));

            res.Add((byte)(data >> 24 & 0xff));

            return res;
        }

        public static List<byte> LoadBytesFromUInt16(ushort data)
        {
            var res = new List<byte>();

            res.Add((byte)(data & 0xff));

            res.Add((byte)(data >> 8 & 0xff));

            return res;
        }

    }
}
