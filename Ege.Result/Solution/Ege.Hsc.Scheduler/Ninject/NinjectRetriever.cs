namespace Ege.Hsc.Scheduler.Ninject
{
    using Ege.Hsc.Scheduler.Retrievers;
    using Quartz;
    using global::Ninject;
    using global::Ege.Hsc.Scheduler.Scheduling;

    internal class NinjectRetriever : ISchedulerRetriever, ISchedulingManagerRetriever
    {
        public static readonly IKernel Kernel = NinjectKernelFactory.CreateKernel();

        public IScheduler GetScheduler()
        {
            return Kernel.Get<IScheduler>();
        }

        public ISchedulingManager GetManager()
        {
            return Kernel.Get<ISchedulingManager>();
        }
    }
}