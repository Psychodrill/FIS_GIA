namespace Ege.Hsc.Scheduler.Jobs
{
    using System.Threading.Tasks;
    using Ege.Hsc.Logic.Requests;
    using JetBrains.Annotations;
    using Quartz;

    class DeleteOldZipsJob : AsyncJob
    {
        [NotNull]private readonly IRequestService _service;

        public DeleteOldZipsJob([NotNull]IRequestService service)
        {
            _service = service;
        }

        protected override async Task InnerExecuteAsync(IJobExecutionContext context)
        {
            var deleted = await _service.DeleteZipsForOldRequests();
            Logger.InfoFormat("Set ZipDeleted status for {0} requests", deleted);
        }
    }
}
