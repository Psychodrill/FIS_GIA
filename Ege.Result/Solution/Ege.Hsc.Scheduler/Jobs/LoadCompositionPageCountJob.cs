namespace Ege.Hsc.Scheduler.Jobs
{
    using System.Threading.Tasks;
    using Ege.Check.Logic.BlankServers;
    using Ege.Check.Logic.Services.Participant;
    using JetBrains.Annotations;
    using Quartz;

    class LoadCompositionPageCountJob : AsyncJob
    {
        [NotNull]private readonly IMemoryCacheService _memoryCacheService;
        [NotNull] private readonly IServerPageCountService _pageCountService;

        public LoadCompositionPageCountJob(
            [NotNull]IMemoryCacheService memoryCacheService, 
            [NotNull]IServerPageCountService pageCountService)
        {
            _memoryCacheService = memoryCacheService;
            _pageCountService = pageCountService;
        }

        protected override async Task InnerExecuteAsync(IJobExecutionContext context)
        {
            await _memoryCacheService.RefreshMemoryCache(
                refreshAnswerCriteria: false,
                refreshRegionSettings: true,
                refreshSubjectsAndExams: true,
                refreshAvailableRegions: false,
                refreshCancelledExams: false);

            await _pageCountService.LoadPageCount();
        }
    }
}
