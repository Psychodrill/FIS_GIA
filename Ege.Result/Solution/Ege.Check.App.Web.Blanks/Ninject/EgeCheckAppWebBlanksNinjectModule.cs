namespace Ege.Check.App.Web.Blanks.Ninject
{
    using Ege.Check.App.Web.Blanks.Esrp;
    using Ege.Hsc.Dal.Entities;
    using global::Ninject.Modules;

    public class EgeCheckAppWebBlanksNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IEsrpUrlCreator>().To<EsrpUrlCreator>().InSingletonScope();
            Bind<IEsrpSettings>().To<EsrpSettings>().InSingletonScope();
            Bind<IUserReferenceCreator>().To<UserReferenceCreator>().InSingletonScope();
        }
    }
}
