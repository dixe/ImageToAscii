using ImageToTextArt.BmpLoader;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace BmpTests
{
    [TestClass]
    public class UnitTest1
    {
        
        [DataTestMethod]
        [DataRow(new byte[] {0x4, 0x3,0x2,0x1})]        
        public void UInt32Tests(byte[] input)
        {
            var uInt = ByteIntConvertion.LoadUInt32FromBytes(input, 0);
            var outData = ByteIntConvertion.LoadBytesFromUInt32(uInt).ToArray();

            Assert.IsTrue(input.Length == outData.Length);

            for (var i = 0; i < input.Length; i++)
            {
                Assert.AreEqual(input[i], outData[i]);
            }
        }



        [DataTestMethod]
        [DataRow(new byte[] { 0x4, 0x3 })]
        public void UInt16Tests(byte[] input)
        {
            var uInt = ByteIntConvertion.LoadUInt16FromBytes(input, 0);
            var outData = ByteIntConvertion.LoadBytesFromUInt16(uInt).ToArray();

            Assert.IsTrue(input.Length == outData.Length);

            for (var i = 0; i < input.Length; i++)
            {
                Assert.AreEqual(input[i], outData[i]);
            }
        }


        [DataTestMethod]
        [DataRow(@"../../../../BmpTests\test.bmp")]
        [DataRow(@"../../../../BmpTests\test2.bmp")]
        public void BmpParseSave(string fileName)
        {
            var bytes = File.ReadAllBytes(fileName);
            var bmp = BmpParser.Parse(bytes);
            var outbytes = bmp.GetFileBytes();
            
            Assert.IsTrue(bytes.Length == outbytes.Length);

            for (var i = 0; i < bytes.Length; i++)
            {
                Assert.AreEqual(bytes[i], outbytes[i]);
            }
        }


        [DataTestMethod]
        [DataRow(@"../../../../BmpTests\test.bmp")]
        [DataRow(@"../../../../BmpTests\test2.bmp")]
        public void BmpFromPixels(string fileName)
        {
            var bytes = File.ReadAllBytes(fileName);
            var bmpFromBytes = BmpParser.Parse(bytes);
            var bmp = BmpParser.FromPixels(bmpFromBytes.Pixels, bmpFromBytes.InfoHeader.BitPerPixel);
            bmp.InfoHeader.BitPerPixel = bmpFromBytes.InfoHeader.BitPerPixel;
            var outbytes = bmp.GetFileBytes();

            Assert.IsTrue(bytes.Length == outbytes.Length);

            for (var i = 0; i < bytes.Length; i++)
            {
                Assert.AreEqual(bytes[i], outbytes[i]);
            }
        }
    }
}
