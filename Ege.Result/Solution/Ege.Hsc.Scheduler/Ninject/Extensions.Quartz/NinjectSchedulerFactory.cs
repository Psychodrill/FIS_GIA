namespace Ege.Hsc.Scheduler.Ninject.Extensions.Quartz
{
    using JetBrains.Annotations;
    using global::Quartz;
    using global::Quartz.Core;
    using global::Quartz.Impl;
    using global::Quartz.Spi;

    public class NinjectSchedulerFactory : StdSchedulerFactory
    {
        private readonly IJobFactory _ninjectJobFactory;

        public NinjectSchedulerFactory([NotNull] IJobFactory ninjectJobFactory)
        {
            _ninjectJobFactory = ninjectJobFactory;
        }

        protected override IScheduler Instantiate([NotNull] QuartzSchedulerResources rsrcs, [NotNull] QuartzScheduler qs)
        {
            qs.JobFactory = _ninjectJobFactory;
            return base.Instantiate(rsrcs, qs);
        }
    }
}