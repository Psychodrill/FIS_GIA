namespace Ege.Check.App.Services.Ninject
{
    using System;
    using Ege.Check.Common.Ninject;
    using Ege.Check.Dal.Blanks.Ninject;
    using Ege.Check.Dal.Cache.Ninject;
    using Ege.Check.Dal.Cache.Redis;
    using Ege.Check.Dal.MemoryCache.Ninject;
    using Ege.Check.Dal.Store.Ninject;
    using Ege.Check.Logic.Ninject;
    using Ege.Check.Logic.Services.Ninject;
    using Ege.Dal.Common.Ninject;
    using JetBrains.Annotations;
    using global::Ninject;

    public static class KernelContainer
    {
        [NotNull] private static readonly Lazy<IKernel> _kernel = new Lazy<IKernel>(Create);

        public static IKernel Kernel
        {
            get { return _kernel.Value; }
        }

        private static IKernel Create()
        {
            return KernelFactoryHelper.CreateKernel(
                new EgeCheckCommonNinjectModule(),
                new EgeDalCommonNinjectModule(),
                new EgeCheckDalStoreNinjectModule(),
                new EgeCheckDalCacheNinjectModule<RedisCacheFactory>(),
                new EgeCheckLogicNinjectModule(),
                new LogicServicesNinjectModule(),
                new AppServicesNinjectModule(),
                new EgeCheckDalMemoryCacheNinjectModule(),
                new EgeCheckDalBlanksNinjectModule()
                );
        }
    }
}
