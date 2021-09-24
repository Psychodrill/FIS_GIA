namespace Ege.Hsc.Logic.Blanks
{
    using System.Threading.Tasks;
    using Ege.Hsc.Logic.Models.Blanks;
    using JetBrains.Annotations;

    public interface IBlankDownloader
    {
        [NotNull]
        Task<BlankDownloadResult> DownloadBlankAsync([NotNull]Blank blankDownload);
    }
}