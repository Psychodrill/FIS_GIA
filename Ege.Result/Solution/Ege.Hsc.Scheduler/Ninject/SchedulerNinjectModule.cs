namespace Ege.Hsc.Scheduler.Ninject
{
    using Ege.Hsc.Scheduler.Retrievers;
    using global::Ninject.Modules;
    using global::Ege.Hsc.Scheduler.Configuration;
    using global::Ege.Hsc.Scheduler.Scheduling;

    internal class SchedulerNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IJobCronStringConfiguration>().To<ConfigurationManagerJobCronStringConfiguration>().InSingletonScope();
            Bind<ISchedulerRetriever>().To<NinjectRetriever>().InSingletonScope();
            Bind<ISchedulingManagerRetriever>().To<NinjectRetriever>().InSingletonScope();
            Bind<ISchedulingManager>().To<SchedulingManager>().InSingletonScope();

        }
    }
}