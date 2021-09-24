namespace Ege.Hsc.Scheduler.Ninject
{
    using Ege.Check.Common.Ninject;
    using Ege.Check.Dal.Blanks.Ninject;
    using Ege.Check.Dal.Cache.Ninject;
    using Ege.Check.Dal.Cache.Redis;
    using Ege.Check.Dal.MemoryCache.Ninject;
    using Ege.Check.Dal.Store.Ninject;
    using Ege.Check.Logic.Ninject;
    using Ege.Dal.Common.Ninject;
    using Ege.Hsc.Dal.Store.Ninject;
    using Ege.Hsc.Logic.Ninject;
    using global::Ninject;
    using global::Ege.Hsc.Scheduler.Ninject.Extensions.Quartz;
    using TsSoft.Excel.Ninject;
    using TsSoft.Excel.NinjectResolver;
    using TsSoft.Expressions.Helpers.Bindings;
    using TsSoft.NinjectBindings;

    internal class NinjectKernelFactory
    {
        public static IKernel CreateKernel()
        {
            return new StandardKernel(
                new SchedulerNinjectModule(),
                new QuartzNinjectModule(),

                new EgeCheckCommonNinjectModule(),
                new EgeDalCommonNinjectModule(),
                new EgeHscDalNinjectModule(),
                new EgeHscLogicNinjectModule(),
                new EgeCheckDalBlanksNinjectModule(),

                new EgeCheckDalStoreNinjectModule(),
                new EgeCheckLogicNinjectModule(),
                new EgeCheckDalCacheNinjectModule<RedisCacheFactory>(),
                new EgeCheckDalMemoryCacheNinjectModule()

                ).CreateBinder().Add<TsSoftExcelNinject>().Add<TsSoftExcelBindingsDescription>().Add<ExpressionsHelpersBindings>().InSingletonScope();
        }
    }
}