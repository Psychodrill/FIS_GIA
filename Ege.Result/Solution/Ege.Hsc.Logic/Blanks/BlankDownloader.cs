namespace Ege.Hsc.Logic.Blanks
{
    using System;
    using System.Net.Http;
    using System.Threading.Tasks;
    using Ege.Hsc.Logic.Models.Blanks;
    using JetBrains.Annotations;

    internal class BlankDownloader : IBlankDownloader
    {
        [NotNull] private readonly IFilePathHelper _filePathHelper;
        [NotNull] private readonly IFileWriter _fileWriter;

        public BlankDownloader([NotNull] IFilePathHelper filePathHelper, [NotNull] IFileWriter fileWriter)
        {
            _filePathHelper = filePathHelper;
            _fileWriter = fileWriter;
        }

        public async Task<BlankDownloadResult> DownloadBlankAsync(Blank blankDownload)
        {
            if (blankDownload.ParticipantHash == null || blankDownload.ParticipantDocumentNumber == null)
            {
                throw new InvalidOperationException(string.Format("Participant {0} without hash or number", blankDownload.ParticipantRbdId));
            }
            var path = _filePathHelper.GetOrCreatePath(
                blankDownload.ParticipantHash,
                blankDownload.ParticipantDocumentNumber,
                blankDownload.ParticipantRbdId,
                blankDownload.Order);
            using (var httpClient = new HttpClient())
            using (var request = new HttpRequestMessage(HttpMethod.Get, blankDownload.Url))
            using (var response = await httpClient.SendAsync(request))
            {
                if (response == null)
                {
                    throw new InvalidOperationException("response is null");
                }
                if (response.Content == null)
                {
                    throw new InvalidOperationException("response.Content is null");
                }
                if (!response.IsSuccessStatusCode)
                {
                    return new BlankDownloadResult(await response.Content.ReadAsStringAsync(), false);
                }
                var success = await _fileWriter.TryWritePngAsync(response.Content, path);
                return success 
                    ? new BlankDownloadResult(true)
                    : new BlankDownloadResult("Not a png file", false);
            }
        }
    }
}
