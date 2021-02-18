
namespace ImageToTextArt.BmpLoader
{
    public class BmpParser
    {

        public static Bmp FromPixels(BmpPixel[,] pixels)
        {

            var header = new BmpHeader {
                DataOffSet = 54,
                FileSize = (uint)(pixels.Length * 4 + 54),
                Reserved = 0,
                Signature = 0x4d42
            };

            var height = (uint)pixels.GetLength(0);
            var width = (uint)pixels.GetLength(1);

            var infoHeader = new BmpInfoHeader
            {
                BitPerPixel = 32,
                ColorsUsed = 0,
                Compression = 0,
                Height = height,
                Width = width,
                ImageSize = 0,
                ImportantColors = 0,
                Planes = 1,
                Size = 40,
                XpixelsPerM = 3780,
                YpixelsPerM = 3780,
            };
            

            var pixelData = pixels;

            return new Bmp
            {
                Header = header,
                InfoHeader = infoHeader,
                Pixels = pixelData
            };
        }

        public static Bmp Parse(byte [] bmpBytes)
        {
            var header = ParseHeader(bmpBytes);
            var infoHeader = ParseBmpInfoHeader(bmpBytes);
            if(infoHeader.BitPerPixel < 8)
            {
                var debug = "Colortable";
            }

            var pixelData = ParseImageData(bmpBytes, header, infoHeader);
            return new Bmp
            {
                Header = header,
                InfoHeader = infoHeader,
                Pixels = pixelData
            };
            
        }

        private static BmpHeader ParseHeader(byte[] bmpBytes)
        {
            var valid = bmpBytes[0] == 'B' && bmpBytes[1] == 'M';

            return new BmpHeader()
            {
                Signature = ByteIntConvertion.LoadUInt16FromBytes(bmpBytes, 0),
                FileSize = ByteIntConvertion.LoadUInt32FromBytes(bmpBytes, 0x2),
                Reserved = ByteIntConvertion.LoadUInt32FromBytes(bmpBytes, 0x6),
                DataOffSet = ByteIntConvertion.LoadUInt32FromBytes(bmpBytes, 0xA)
            };

        }

        private static BmpInfoHeader ParseBmpInfoHeader(byte[] bmpBytes)
        {
            var size = ByteIntConvertion.LoadUInt32FromBytes(bmpBytes, 0xE);

            if(size != 40)
            {
                var debug = "error";
            }
            var colorsUsed = ByteIntConvertion.LoadUInt32FromBytes(bmpBytes, 0x2E);

            if (colorsUsed != 256)
            {
                var debug = "error";
            }

            var imageSize = ByteIntConvertion.LoadUInt32FromBytes(bmpBytes, 0x22);

            return new BmpInfoHeader()
            {
                Size = size,
                Width = ByteIntConvertion.LoadUInt32FromBytes(bmpBytes, 0x12),
                Height = ByteIntConvertion.LoadUInt32FromBytes(bmpBytes, 0x16),
                Planes = ByteIntConvertion.LoadUInt16FromBytes(bmpBytes, 0x1A),
                BitPerPixel = ByteIntConvertion.LoadUInt16FromBytes(bmpBytes, 0x1C),
                Compression = ByteIntConvertion.LoadUInt32FromBytes(bmpBytes, 0x1E),
                ImageSize = imageSize,
                XpixelsPerM = ByteIntConvertion.LoadUInt32FromBytes(bmpBytes, 0x26),
                YpixelsPerM = ByteIntConvertion.LoadUInt32FromBytes(bmpBytes, 0x2A),
                ColorsUsed = colorsUsed,
                ImportantColors = ByteIntConvertion.LoadUInt32FromBytes(bmpBytes, 0x32),
            };

        }


        private static BmpPixel[,] ParseImageData(byte[] bmpBytes, BmpHeader header, BmpInfoHeader infoHeader)
        {
            if (infoHeader.Compression != 0)
            {
                var debug = "compression";
            }

            var offSet = header.DataOffSet;
            var pixelCount = infoHeader.ImageSize > 0 ? infoHeader.ImageSize : infoHeader.Width * infoHeader.Height;

            var pixels = new BmpPixel[infoHeader.Height, infoHeader.Width];
            var byteSteps = infoHeader.BitPerPixel / 8;
            for (var i = 0; i < pixelCount; i++)
            {
                var byteIndex = (i * byteSteps) + offSet;
                var y = infoHeader.Height - 1 -  i / infoHeader.Width;
                var x = i % infoHeader.Width;
                var pixel = new BmpPixel
                {
                    Blue = bmpBytes[byteIndex + 0],
                    Green = bmpBytes[byteIndex + 1],
                    Red = bmpBytes[byteIndex + 2]
                };
                if(infoHeader.BitPerPixel >= 32)
                {
                    pixel.Alpha = bmpBytes[byteIndex + 2];

                }
                else
                {
                    pixel.Alpha = 255;
                }
                    
                pixels[y, x] = pixel;

            }

            return pixels;
        }
    }
}
