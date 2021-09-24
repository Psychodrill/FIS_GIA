namespace Ege.Check.App.Web.Ninject
{
    using Ege.Check.App.Web.Path;
    using Ege.Check.Common.Path;
    using global::Ninject.Modules;

    public class EgeCheckAppWebNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IPathResolver>().To<PathResolver>().InSingletonScope();
        }
    }
}
