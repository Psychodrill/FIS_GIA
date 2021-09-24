namespace Ege.Hsc.Scheduler.Jobs
{
    using System.Threading.Tasks;
    using Ege.Hsc.Logic.Blanks;
    using JetBrains.Annotations;
    using Quartz;

    class FixPageCountInconsistenciesJob : AsyncJob
    {
        [NotNull]private readonly IBlankService _blankService;

        public FixPageCountInconsistenciesJob([NotNull]IBlankService blankService)
        {
            _blankService = blankService;
        }

        protected override async Task InnerExecuteAsync(IJobExecutionContext context)
        {
            await _blankService.FixInconsistenciesWithCheckEgeDb();
        }
    }
}
