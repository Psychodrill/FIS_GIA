namespace Ege.Hsc.Scheduler.Jobs
{
    using System.Threading.Tasks;
    using Ege.Check.Logic.Services.Participant;
    using JetBrains.Annotations;
    using Quartz;

    class UpdateBlankCacheJob : AsyncJob
    {
        [NotNull]
        private readonly ICacheUpdaterService _updaterService;

        public UpdateBlankCacheJob([NotNull]ICacheUpdaterService updaterService)
        {
            _updaterService = updaterService;
        }

        protected override Task InnerExecuteAsync(IJobExecutionContext context)
        {
            return _updaterService.UpdateBlankCompositionPageCount();
        }
    }
}
