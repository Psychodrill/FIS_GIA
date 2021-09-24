namespace Ege.Hsc.Logic.Blanks
{
    using System.IO;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Common.Logging;
    using JetBrains.Annotations;

    internal class FileWriter : IFileWriter
    {
        [NotNull]
        private static readonly ILog Logger = LogManager.GetLogger<FileWriter>();

        public async Task<bool> TryWritePngAsync(HttpContent content, string path)
        {
            if (File.Exists(path))
            {
                Logger.WarnFormat("File {0} already exists, it will be deleted", path);
                File.Delete(path);
            }
            using (var contentStream = await content.ReadAsStreamAsync())
            {
                var buffer = new byte[MagicNumbers.Png.Length];
                contentStream.Read(buffer, 0, buffer.Length);
                for (int i = 0; i < MagicNumbers.Png.Length; ++i)
                {
                    if (buffer[i] != MagicNumbers.Png[i])
                    {
                        return false;
                    }
                }
                using (var fileStream = new FileStream(path, FileMode.CreateNew, FileAccess.Write, FileShare.None))
                {
                    await fileStream.WriteAsync(MagicNumbers.Png, 0, MagicNumbers.Png.Length);
                    await contentStream.CopyToAsync(fileStream);
                }
                return true;
            }
        }
    }
}
