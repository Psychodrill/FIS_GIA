namespace Ege.Hsc.Logic.Blanks
{
    using System.Net.Http;
    using System.Threading.Tasks;
    using JetBrains.Annotations;

    internal interface IFileWriter
    {
        [NotNull]
        Task<bool> TryWritePngAsync([NotNull]HttpContent content, [NotNull]string path);
    }
}
