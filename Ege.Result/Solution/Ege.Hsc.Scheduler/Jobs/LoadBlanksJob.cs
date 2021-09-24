namespace Ege.Hsc.Scheduler.Jobs
{
    using System.Threading.Tasks;
    using Ege.Hsc.Logic.Blanks;
    using JetBrains.Annotations;
    using Quartz;

    class LoadBlanksJob : AsyncJob
    {
        [NotNull] private readonly IBlankService _service;

        public LoadBlanksJob([NotNull]IBlankService service)
        {
            _service = service;
        }

        protected async override Task InnerExecuteAsync(IJobExecutionContext context)
        {
            await _service.LoadBlanksFromCheckEgeDb();
        }
    }
}
