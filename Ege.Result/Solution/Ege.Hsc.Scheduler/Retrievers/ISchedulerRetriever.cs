namespace Ege.Hsc.Scheduler.Retrievers
{
    using JetBrains.Annotations;
    using Quartz;

    internal interface ISchedulerRetriever
    {
        [NotNull]
        IScheduler GetScheduler();
    }
}