namespace Ege.Check.App.Services.Ninject
{
    using System.ServiceModel;
    using global::Ninject.Extensions.Wcf;
    using global::Ninject.Modules;

    public class AppServicesNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind(typeof (ServiceHost)).To(typeof (NinjectServiceHost)).InSingletonScope();
        }
    }
}