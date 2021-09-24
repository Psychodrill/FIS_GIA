namespace Ege.Hsc.Scheduler.Jobs
{
    using System;
    using System.Data.Common;
    using System.Threading.Tasks;
    using Ege.Check.Dal.Store.Factory;
    using Ege.Hsc.Dal.Entities;
    using Ege.Hsc.Dal.Store.Repositories;
    using Ege.Hsc.Logic.Blanks;
    using JetBrains.Annotations;
    using Quartz;

    internal class DownloadBlanksJob : AsyncJob
    {
        [NotNull] private readonly IBlankDownloadRepository _blankDownloadRepository;
        [NotNull] private readonly IBlankDownloader _blankDownloader;
        [NotNull] private readonly IBlankService _blankService;
        [NotNull] private readonly IConnectionFactory<DbConnection> _dbConnectionFactory;

        public DownloadBlanksJob([NotNull] IBlankDownloader blankDownloader,
                                 [NotNull] IBlankService blankService,
                                 [NotNull] IBlankDownloadRepository blankDownloadRepository,
                                 [NotNull] IConnectionFactory<DbConnection> dbConnectionFactory)
        {
            _blankDownloader = blankDownloader;
            _blankService = blankService;
            _blankDownloadRepository = blankDownloadRepository;
            _dbConnectionFactory = dbConnectionFactory;
        }

        protected override async Task InnerExecuteAsync(IJobExecutionContext context)
        {
            var blanks = await _blankService.BlanksToDownload();
            if (blanks == null)
            {
                Logger.TraceFormat("Not available blanks to download");
                return;
            }
            Logger.TraceFormat("Got {0} blanks to download", blanks.Count);
            foreach (var blank in blanks)
            {
                if (blank == null)
                {
                    throw new NullReferenceException(
                        "Null blank in blank collection from IBlankService::BlanksToDownload");
                }
                var downloadSuccessfully = await _blankDownloader.DownloadBlankAsync(blank);
                BlankDownloadState state;
                if (!downloadSuccessfully.Successfully)
                {
                    Logger.WarnFormat("Can not download blank {0} from url {1}.\r\n Message: {2}", blank.Id, blank.Url,
                                      downloadSuccessfully.Message);
                    state = BlankDownloadState.Error;
                }
                else
                {
                    state = BlankDownloadState.Downloaded;
                }
                using (var connection = await _dbConnectionFactory.CreateHscAsync())
                {
                    await _blankDownloadRepository.ChangeStateAsync(connection, blank.Id, state);
                }
            }
        }
    }
}
