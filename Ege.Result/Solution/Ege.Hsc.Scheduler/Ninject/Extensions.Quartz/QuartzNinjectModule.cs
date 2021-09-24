namespace Ege.Hsc.Scheduler.Ninject.Extensions.Quartz
{
    using global::Ninject;
    using global::Ninject.Modules;
    using global::Quartz;
    using global::Quartz.Spi;

    public class QuartzNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IJobFactory>().To<NinjectJobFactory>().InSingletonScope();
            Bind<ISchedulerFactory>().To<NinjectSchedulerFactory>().InSingletonScope();
            Bind<IScheduler>().ToMethod(ctx => ctx.Kernel.Get<ISchedulerFactory>().GetScheduler()).InSingletonScope();
        }
    }
}