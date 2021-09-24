namespace Ege.Check.App.Web.Ninject
{
    using Ege.Check.App.Web.Common.Ninject;
    using Ege.Check.Captcha.Ninject;
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
    using TsSoft.Excel.Ninject;
    using TsSoft.Excel.NinjectResolver;
    using TsSoft.Expressions.Helpers.Bindings;
    using TsSoft.NinjectBindings;

    public static class WebAppKernelFactory
    {
        public static IKernel CreateKernel()
        {
            return KernelFactoryHelper.CreateKernel(
                new EgeCheckCommonNinjectModule(),
                new EgeCheckAppWebNinjectModule(),
                new EgeDalCommonNinjectModule(),
                new EgeCheckDalStoreNinjectModule(),
                new EgeCheckDalCacheNinjectModule<RedisCacheFactory>(),
                new EgeCheckLogicNinjectModule(),
                new EgeCheckCaptchaNinjectModule(),
                new EgeCheckDalBlanksNinjectModule(),
                new EgeCheckDalMemoryCacheNinjectModule(),
                new EgeCheckAppWebCommonNinjectModule(),
                new EgeHscLogicNinjectModule(),
                new EgeHscDalNinjectModule()).CreateBinder().Add<TsSoftExcelNinject>().Add<TsSoftExcelBindingsDescription>().Add<ExpressionsHelpersBindings>().InSingletonScope();
        }
    }
}