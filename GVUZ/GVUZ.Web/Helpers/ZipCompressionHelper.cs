using System.IO;
using System.IO.Compression;
using System.Security.Cryptography;
using System.Text;

namespace GVUZ.Web.Helpers
{
    public static class ZipCompressionHelper
    {
        public static string ZipToBase64String(string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return null;
            }

            using (var ms = new MemoryStream())
            {
                using (var bs64 = new ToBase64Transform())
                {
                    using (var cs = new CryptoStream(ms, bs64, CryptoStreamMode.Write))
                    {
                        using (var zs = new GZipStream(cs, CompressionMode.Compress))
                        {
                            using (StreamWriter sw = new StreamWriter(zs))
                            {
                                sw.Write(source);
                            }
                        }
                    }
                }

                return Encoding.ASCII.GetString(ms.ToArray());
            }
        }

        public static string UnzipFromBase64String(string base64Zipped)
        {
            if (string.IsNullOrEmpty(base64Zipped))
            {
                return null;
            }

            using (var ms = new MemoryStream(Encoding.ASCII.GetBytes(base64Zipped)))
            {
                using (var bs64 = new FromBase64Transform())
                {
                    using (var cs = new CryptoStream(ms, bs64, CryptoStreamMode.Read))
                    {
                        using (var zs = new GZipStream(cs, CompressionMode.Decompress, false))
                        {
                            using (StreamReader reader = new StreamReader(zs))
                            {
                                return reader.ReadToEnd();
                            }
                        }
                    }
                }
            }
        }
    }
}