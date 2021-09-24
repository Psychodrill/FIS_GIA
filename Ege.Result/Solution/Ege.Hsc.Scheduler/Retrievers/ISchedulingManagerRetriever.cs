namespace Ege.Hsc.Scheduler.Retrievers
{
    using Ege.Hsc.Scheduler.Scheduling;
    using JetBrains.Annotations;

    internal interface ISchedulingManagerRetriever
    {
        [NotNull]
        ISchedulingManager GetManager();
    }
}