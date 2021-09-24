namespace Ege.Check.Logic.LoadServices
{
    using System.IO;
    using System.IO.Compression;
    using System.Text;
    using System.Threading.Tasks;
    using Ege.Check.Logic.LoadServices.Preprocessing;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class DecompressorTests
    {
        [TestMethod]
        public async Task DecompressTest()
        {
            var decompressor = new Decompressor();
            byte[] compressed;
            const string expectedString = "ddfhdfthgiuysdt98ghr5y7e89tu hsdf89pgys89d8gusdr9g";
            var expected = Encoding.UTF8.GetBytes(expectedString);

            using (var inStream = new MemoryStream(expected))
            using (var outStream = new MemoryStream())
            {
                using (var zipStream = new GZipStream(outStream, CompressionMode.Compress, true))
                {
                    await inStream.CopyToAsync(zipStream);
                }
                compressed = outStream.ToArray();
            }


            using (var actual = await decompressor.DecompressAsync(compressed))
            using (var ms = new MemoryStream())
            {
                await actual.CopyToAsync(ms);
                ms.Position = 0;
                var actualArray = ms.ToArray();
                CollectionAssert.AreEqual(expected, actualArray);
                CollectionAssert.AreNotEqual(compressed, actualArray);
                Assert.AreEqual(expectedString, Encoding.UTF8.GetString(actualArray));
            }
        }

        [TestMethod]
        public async Task CompressTest()
        {
            var decompressor = new Decompressor();
            const string expectedString = "fffffffffffff";
            var expected = Encoding.UTF8.GetBytes(expectedString);

            var compressed = await decompressor.CompressAsync(new MemoryStream(expected));

            using (var expectedStream = await decompressor.DecompressAsync(compressed))
            using (var ms = new MemoryStream())
            {
                await expectedStream.CopyToAsync(ms);
                ms.Position = 0;
                var actualArray = ms.ToArray();
                CollectionAssert.AreEqual(expected, actualArray);
                CollectionAssert.AreNotEqual(compressed, actualArray);
                Assert.AreEqual(expectedString, Encoding.UTF8.GetString(actualArray));
            }
        }
    }
}