namespace Ege.Hsc.Scheduler.Jobs
{
    using System.Threading.Tasks;
    using Ege.Check.Common.Async;
    using Quartz;

    internal abstract class AsyncJob : BaseJob
    {
        protected override void InnerExecute(IJobExecutionContext context)
        {
            AsyncHelper.RunSync(() => InnerExecuteAsync(context));
        }

        protected abstract Task InnerExecuteAsync(IJobExecutionContext context);
    }
}