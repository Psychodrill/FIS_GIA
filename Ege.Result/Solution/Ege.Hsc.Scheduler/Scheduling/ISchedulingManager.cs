namespace Ege.Hsc.Scheduler.Scheduling
{
    using System.Configuration;

    internal interface ISchedulingManager
    {
        void StartScheduler();
        void StopScheduler();
    }
}