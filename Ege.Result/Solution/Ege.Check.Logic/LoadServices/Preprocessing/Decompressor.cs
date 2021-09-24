namespace Ege.Check.Logic.LoadServices.Preprocessing
{
    using System.IO;
    using System.IO.Compression;
    using System.Threading.Tasks;

    public class Decompressor : IDecompressor, ICompressor
    {
        public async Task<byte[]> CompressAsync(Stream source)
        {
            using (var output = new MemoryStream())
            {
                using (var zip = new GZipStream(output, CompressionMode.Compress))
                {
                    await source.CopyToAsync(zip);
                }
                return output.ToArray();
            }
        }

        public async Task<Stream> DecompressAsync(byte[] source)
        {
            using (var inStream = new MemoryStream(source))
            {
                var streamOut = new MemoryStream();
                using (var zipStream = new GZipStream(inStream, CompressionMode.Decompress))
                {
                    await zipStream.CopyToAsync(streamOut);
                }
                streamOut.Position = 0;
                return streamOut;
            }
        }
    }
}