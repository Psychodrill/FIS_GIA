namespace Ege.Check.Logic.Services.Ninject
{
    using Ege.Check.Logic.Services.Inspectors;
    using Ege.Check.Logic.Services.Load;
    using global::Ninject.Modules;

    public class LogicServicesNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind(typeof (IDataLoader<>)).To(typeof (DataLoader<>)).InSingletonScope();
            Bind<IRequestLogWriter>().To<RequestLogWriter>().InSingletonScope();
        }
    }
}