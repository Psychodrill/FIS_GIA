namespace Ege.Hsc.Scheduler.Jobs
{
    using System.Threading.Tasks;
    using Ege.Hsc.Logic.Servers;
    using JetBrains.Annotations;
    using Quartz;

    class CheckServerAvailabilityJob : AsyncJob
    {
        [NotNull]private readonly IServerService _serverService;

        public CheckServerAvailabilityJob([NotNull]IServerService serverService)
        {
            _serverService = serverService;
        }

        protected override async Task InnerExecuteAsync(IJobExecutionContext context)
        {
            await _serverService.CheckServersAvailability();
        }
    }
}
