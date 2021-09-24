namespace Ege.Check.Common.Ninject
{
    using Ege.Check.Common.Config;
    using Ege.Check.Common.Hash;
    using Ege.Check.Common.Http;
    using Ege.Check.Common.Random;
    using global::Ninject.Modules;

    public class EgeCheckCommonNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IRandomCreator>().To<RandomCreator>().InSingletonScope();
            Bind<IFioHasher>().To<FioHasher>().InSingletonScope();
            Bind<IConfigReaderHelper>().To<ConfigReaderHelper>().InSingletonScope();
            Bind<IHttpFileLoader>().To<HttpFileLoader>().InSingletonScope();
        }
    }
}