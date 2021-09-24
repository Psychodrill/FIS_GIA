namespace Ege.Check.Dal.Blanks.Ninject
{
    using global::Ninject.Modules;

    public class EgeCheckDalBlanksNinjectModule : NinjectModule
    {
        public override void Load()
        {
            Bind<IBlankModelCreator>().To<BlankModelCreator>().InSingletonScope();
        }
    }
}